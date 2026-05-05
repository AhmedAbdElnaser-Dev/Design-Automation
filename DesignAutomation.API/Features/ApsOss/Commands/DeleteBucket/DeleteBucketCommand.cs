using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.DeleteBucket;

public record DeleteBucketCommand(string BucketKey) : IRequest<Unit>;
