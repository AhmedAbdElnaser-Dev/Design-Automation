using DesignAutomation.API.Features.ApsOss.Commands.CreateBucket;
using DesignAutomation.API.Features.ApsOss.Commands.DeleteBucket;
using DesignAutomation.API.Features.ApsOss.Commands.DeleteObject;
using DesignAutomation.API.Features.ApsOss.Commands.UploadObject;
using DesignAutomation.API.Features.ApsOss.Queries.GetBucket;
using DesignAutomation.API.Features.ApsOss.Queries.GetSignedDownload;
using DesignAutomation.API.Features.ApsOss.Queries.ListBuckets;
using DesignAutomation.API.Features.ApsOss.Queries.ListObjects;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignAutomation.API.Controllers;

[ApiController]
[Route("api/aps/oss")]
[Authorize]
public class ApsOssController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApsOssController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("buckets")]
    public async Task<ActionResult<BucketListViewModel>> ListBuckets(
        [FromQuery] string? region,
        [FromQuery] int? limit,
        [FromQuery] string? startAt,
        CancellationToken ct)
        => Ok(await _mediator.Send(new ListBucketsQuery(region, limit, startAt), ct));

    [HttpGet("buckets/{bucketKey}")]
    public async Task<ActionResult<BucketViewModel>> GetBucket(string bucketKey, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBucketQuery(bucketKey), ct));

    [HttpPost("buckets")]
    public async Task<ActionResult<BucketViewModel>> CreateBucket(CreateBucketCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));

    [HttpDelete("buckets/{bucketKey}")]
    public async Task<IActionResult> DeleteBucket(string bucketKey, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBucketCommand(bucketKey), ct);
        return NoContent();
    }

    [HttpGet("buckets/{bucketKey}/objects")]
    public async Task<ActionResult<ObjectListViewModel>> ListObjects(
        string bucketKey,
        [FromQuery] int? limit,
        [FromQuery] string? startAt,
        CancellationToken ct)
        => Ok(await _mediator.Send(new ListObjectsQuery(bucketKey, limit, startAt), ct));

    [HttpPost("buckets/{bucketKey}/objects")]
    [RequestSizeLimit(5L * 1024 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024)]
    public async Task<ActionResult<ObjectViewModel>> UploadObject(
        string bucketKey,
        IFormFile file,
        [FromQuery] string? objectKey,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File is required.");

        var key = string.IsNullOrWhiteSpace(objectKey) ? file.FileName : objectKey;
        await using var stream = file.OpenReadStream();
        var command = new UploadObjectCommand(bucketKey, key, stream, file.Length, file.ContentType);
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpGet("buckets/{bucketKey}/objects/{objectKey}/download-url")]
    public async Task<ActionResult<SignedDownloadViewModel>> GetSignedDownload(
        string bucketKey,
        string objectKey,
        [FromQuery] int? minutesExpiration,
        CancellationToken ct)
        => Ok(await _mediator.Send(new GetSignedDownloadQuery(bucketKey, objectKey, minutesExpiration), ct));

    [HttpDelete("buckets/{bucketKey}/objects/{objectKey}")]
    public async Task<IActionResult> DeleteObject(string bucketKey, string objectKey, CancellationToken ct)
    {
        await _mediator.Send(new DeleteObjectCommand(bucketKey, objectKey), ct);
        return NoContent();
    }
}
