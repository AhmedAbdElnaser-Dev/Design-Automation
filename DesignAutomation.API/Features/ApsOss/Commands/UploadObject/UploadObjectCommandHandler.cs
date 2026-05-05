using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.UploadObject;

public class UploadObjectCommandHandler : IRequestHandler<UploadObjectCommand, ObjectViewModel>
{
    private readonly IApsOssService _oss;

    public UploadObjectCommandHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<ObjectViewModel> Handle(UploadObjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _oss.UploadObjectAsync(
            request.BucketKey,
            request.ObjectKey,
            request.Content,
            request.ContentLength,
            request.ContentType,
            cancellationToken);
        return result.ToViewModel();
    }
}
