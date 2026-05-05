using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.DeleteBucket;

public class DeleteBucketCommandHandler : IRequestHandler<DeleteBucketCommand, Unit>
{
    private readonly IApsOssService _oss;

    public DeleteBucketCommandHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<Unit> Handle(DeleteBucketCommand request, CancellationToken cancellationToken)
    {
        await _oss.DeleteBucketAsync(request.BucketKey, cancellationToken);
        return Unit.Value;
    }
}
