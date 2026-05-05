using DesignAutomation.API.Services.Aps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignAutomation.API.Controllers;

[ApiController]
[Route("api/aps/oss")]
[Authorize]
public class ApsOssController : ControllerBase
{
    private readonly IApsOssService _oss;

    public ApsOssController(IApsOssService oss)
    {
        _oss = oss;
    }

    public class CreateBucketDto
    {
        public string BucketKey { get; set; } = default!;
        public string? PolicyKey { get; set; }
        public string? Region { get; set; }
    }

    [HttpGet("buckets")]
    public async Task<ActionResult<BucketListResponse>> ListBuckets(
        [FromQuery] string? region,
        [FromQuery] int? limit,
        [FromQuery] string? startAt,
        CancellationToken ct)
        => Ok(await _oss.ListBucketsAsync(region, limit, startAt, ct));

    [HttpGet("buckets/{bucketKey}")]
    public async Task<ActionResult<BucketDetails>> GetBucket(string bucketKey, CancellationToken ct)
        => Ok(await _oss.GetBucketDetailsAsync(bucketKey, ct));

    [HttpPost("buckets")]
    public async Task<ActionResult<BucketDetails>> CreateBucket([FromBody] CreateBucketDto dto, CancellationToken ct)
        => Ok(await _oss.CreateBucketAsync(dto.BucketKey, dto.PolicyKey, dto.Region, ct));

    [HttpDelete("buckets/{bucketKey}")]
    public async Task<IActionResult> DeleteBucket(string bucketKey, CancellationToken ct)
    {
        await _oss.DeleteBucketAsync(bucketKey, ct);
        return NoContent();
    }

    [HttpGet("buckets/{bucketKey}/objects")]
    public async Task<ActionResult<ObjectListResponse>> ListObjects(
        string bucketKey,
        [FromQuery] int? limit,
        [FromQuery] string? startAt,
        CancellationToken ct)
        => Ok(await _oss.ListObjectsAsync(bucketKey, limit, startAt, ct));

    [HttpPost("buckets/{bucketKey}/objects")]
    [RequestSizeLimit(5L * 1024 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024)]
    public async Task<ActionResult<ObjectDetails>> UploadObject(
        string bucketKey,
        IFormFile file,
        [FromQuery] string? objectKey,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest("File is required.");
        var key = string.IsNullOrWhiteSpace(objectKey) ? file.FileName : objectKey;
        await using var stream = file.OpenReadStream();
        var result = await _oss.UploadObjectAsync(bucketKey, key, stream, file.Length, file.ContentType, ct);
        return Ok(result);
    }

    [HttpGet("buckets/{bucketKey}/objects/{objectKey}/download-url")]
    public async Task<ActionResult<SignedS3DownloadResponse>> GetSignedDownload(
        string bucketKey,
        string objectKey,
        [FromQuery] int? minutesExpiration,
        CancellationToken ct)
        => Ok(await _oss.GetSignedDownloadAsync(bucketKey, objectKey, minutesExpiration, ct));

    [HttpDelete("buckets/{bucketKey}/objects/{objectKey}")]
    public async Task<IActionResult> DeleteObject(string bucketKey, string objectKey, CancellationToken ct)
    {
        await _oss.DeleteObjectAsync(bucketKey, objectKey, ct);
        return NoContent();
    }
}
