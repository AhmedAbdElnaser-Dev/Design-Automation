using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.DeleteObject;

public class DeleteObjectCommandHandler : IRequestHandler<DeleteObjectCommand, Unit>
{
    private readonly IApsOssService _oss;

    public DeleteObjectCommandHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<Unit> Handle(DeleteObjectCommand request, CancellationToken cancellationToken)
    {
        await _oss.DeleteObjectAsync(request.BucketKey, request.ObjectKey, cancellationToken);
        return Unit.Value;
    }
}
