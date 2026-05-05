using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace DesignAutomation.API.Services.Aps;

public class ApsOssService : IApsOssService
{
    private const long ChunkSize = 5 * 1024 * 1024;

    private readonly HttpClient _http;
    private readonly IApsAuthService _auth;
    private readonly ApsOptions _options;
    private readonly ILogger<ApsOssService> _logger;

    public ApsOssService(HttpClient http, IApsAuthService auth, IOptions<ApsOptions> options, ILogger<ApsOssService> logger)
    {
        _http = http;
        _auth = auth;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<BucketListResponse> ListBucketsAsync(string? region = null, int? limit = null, string? startAt = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (!string.IsNullOrEmpty(region)) qs.Add($"region={Uri.EscapeDataString(region)}");
        if (limit.HasValue) qs.Add($"limit={limit.Value}");
        if (!string.IsNullOrEmpty(startAt)) qs.Add($"startAt={Uri.EscapeDataString(startAt)}");
        var url = "/oss/v2/buckets" + (qs.Count > 0 ? "?" + string.Join("&", qs) : string.Empty);

        using var req = await BuildAsync(HttpMethod.Get, url, ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
        return await resp.Content.ReadFromJsonAsync<BucketListResponse>(cancellationToken: ct)
               ?? new BucketListResponse();
    }

    public async Task<BucketDetails> GetBucketDetailsAsync(string bucketKey, CancellationToken ct = default)
    {
        using var req = await BuildAsync(HttpMethod.Get, $"/oss/v2/buckets/{bucketKey}/details", ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
        return await resp.Content.ReadFromJsonAsync<BucketDetails>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty bucket details response.");
    }

    public async Task<BucketDetails> CreateBucketAsync(string bucketKey, string? policyKey = null, string? region = null, CancellationToken ct = default)
    {
        var body = new CreateBucketRequest
        {
            BucketKey = bucketKey.ToLowerInvariant(),
            PolicyKey = policyKey ?? _options.DefaultBucketPolicy,
        };

        using var req = await BuildAsync(HttpMethod.Post, "/oss/v2/buckets", ct);
        if (!string.IsNullOrEmpty(region))
            req.Headers.Add("x-ads-region", region);
        req.Content = JsonContent.Create(body);

        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
        return await resp.Content.ReadFromJsonAsync<BucketDetails>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty create-bucket response.");
    }

    public async Task DeleteBucketAsync(string bucketKey, CancellationToken ct = default)
    {
        using var req = await BuildAsync(HttpMethod.Delete, $"/oss/v2/buckets/{bucketKey}", ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
    }

    public async Task<ObjectListResponse> ListObjectsAsync(string bucketKey, int? limit = null, string? startAt = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (limit.HasValue) qs.Add($"limit={limit.Value}");
        if (!string.IsNullOrEmpty(startAt)) qs.Add($"startAt={Uri.EscapeDataString(startAt)}");
        var url = $"/oss/v2/buckets/{bucketKey}/objects" + (qs.Count > 0 ? "?" + string.Join("&", qs) : string.Empty);

        using var req = await BuildAsync(HttpMethod.Get, url, ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
        return await resp.Content.ReadFromJsonAsync<ObjectListResponse>(cancellationToken: ct)
               ?? new ObjectListResponse();
    }

    public async Task<ObjectDetails> UploadObjectAsync(string bucketKey, string objectKey, Stream content, long contentLength, string? contentType = null, CancellationToken ct = default)
    {
        var totalParts = (int)Math.Max(1, (contentLength + ChunkSize - 1) / ChunkSize);
        var encodedObject = Uri.EscapeDataString(objectKey);

        SignedS3UploadResponse? signed = null;
        var uploadedParts = 0;

        var buffer = new byte[ChunkSize];
        while (uploadedParts < totalParts)
        {
            var partsToRequest = Math.Min(25, totalParts - uploadedParts);
            using (var initReq = await BuildAsync(HttpMethod.Get,
                       $"/oss/v2/buckets/{bucketKey}/objects/{encodedObject}/signeds3upload?parts={partsToRequest}&firstPart={uploadedParts + 1}" +
                       (signed?.UploadKey is { } k ? $"&uploadKey={Uri.EscapeDataString(k)}" : string.Empty),
                       ct))
            using (var initResp = await _http.SendAsync(initReq, ct))
            {
                await EnsureSuccess(initResp, ct);
                signed = await initResp.Content.ReadFromJsonAsync<SignedS3UploadResponse>(cancellationToken: ct)
                         ?? throw new InvalidOperationException("Empty signed S3 upload response.");
            }

            for (var i = 0; i < signed.Urls.Count; i++)
            {
                var bytesRemaining = contentLength - uploadedParts * ChunkSize;
                var bytesToRead = (int)Math.Min(ChunkSize, bytesRemaining);
                var read = 0;
                while (read < bytesToRead)
                {
                    var n = await content.ReadAsync(buffer.AsMemory(read, bytesToRead - read), ct);
                    if (n == 0) break;
                    read += n;
                }

                using var partContent = new ByteArrayContent(buffer, 0, read);
                using var partResp = await _http.PutAsync(signed.Urls[i], partContent, ct);
                if (!partResp.IsSuccessStatusCode)
                {
                    var body = await partResp.Content.ReadAsStringAsync(ct);
                    throw new HttpRequestException($"S3 part upload failed ({(int)partResp.StatusCode}): {body}");
                }
                uploadedParts++;
            }
        }

        var complete = new SignedS3UploadCompleteRequest
        {
            UploadKey = signed!.UploadKey,
            Size = contentLength,
            ContentType = contentType,
        };

        using var finalReq = await BuildAsync(HttpMethod.Post, $"/oss/v2/buckets/{bucketKey}/objects/{encodedObject}/signeds3upload", ct);
        finalReq.Content = JsonContent.Create(complete);
        using var finalResp = await _http.SendAsync(finalReq, ct);
        await EnsureSuccess(finalResp, ct);
        return await finalResp.Content.ReadFromJsonAsync<ObjectDetails>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty finalize-upload response.");
    }

    public async Task<SignedS3DownloadResponse> GetSignedDownloadAsync(string bucketKey, string objectKey, int? minutesExpiration = null, CancellationToken ct = default)
    {
        var encodedObject = Uri.EscapeDataString(objectKey);
        var url = $"/oss/v2/buckets/{bucketKey}/objects/{encodedObject}/signeds3download" +
                  (minutesExpiration.HasValue ? $"?minutesExpiration={minutesExpiration.Value}" : string.Empty);

        using var req = await BuildAsync(HttpMethod.Get, url, ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
        return await resp.Content.ReadFromJsonAsync<SignedS3DownloadResponse>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty signed S3 download response.");
    }

    public async Task DeleteObjectAsync(string bucketKey, string objectKey, CancellationToken ct = default)
    {
        var encodedObject = Uri.EscapeDataString(objectKey);
        using var req = await BuildAsync(HttpMethod.Delete, $"/oss/v2/buckets/{bucketKey}/objects/{encodedObject}", ct);
        using var resp = await _http.SendAsync(req, ct);
        await EnsureSuccess(resp, ct);
    }

    private async Task<HttpRequestMessage> BuildAsync(HttpMethod method, string relativeUrl, CancellationToken ct)
    {
        var token = await _auth.GetAccessTokenAsync(ct);
        var req = new HttpRequestMessage(method, relativeUrl);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return req;
    }

    private static async Task EnsureSuccess(HttpResponseMessage resp, CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;
        var body = await resp.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException($"APS request failed ({(int)resp.StatusCode}): {body}", null, resp.StatusCode);
    }
}
