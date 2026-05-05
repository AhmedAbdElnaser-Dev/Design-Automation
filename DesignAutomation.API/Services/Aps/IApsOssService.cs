using DesignAutomation.API.Services.Aps.Models;

namespace DesignAutomation.API.Services.Aps;

public interface IApsOssService
{
    Task<BucketListResponse> ListBucketsAsync(string? region = null, int? limit = null, string? startAt = null, CancellationToken ct = default);
    Task<BucketDetailsResponse> GetBucketDetailsAsync(string bucketKey, CancellationToken ct = default);
    Task<BucketDetailsResponse> CreateBucketAsync(string bucketKey, string? policyKey = null, string? region = null, CancellationToken ct = default);
    Task DeleteBucketAsync(string bucketKey, CancellationToken ct = default);

    Task<ObjectListResponse> ListObjectsAsync(string bucketKey, int? limit = null, string? startAt = null, CancellationToken ct = default);
    Task<ObjectDetailsResponse> UploadObjectAsync(string bucketKey, string objectKey, Stream content, long contentLength, string? contentType = null, CancellationToken ct = default);
    Task<SignedS3DownloadResponse> GetSignedDownloadAsync(string bucketKey, string objectKey, int? minutesExpiration = null, CancellationToken ct = default);
    Task DeleteObjectAsync(string bucketKey, string objectKey, CancellationToken ct = default);
}
