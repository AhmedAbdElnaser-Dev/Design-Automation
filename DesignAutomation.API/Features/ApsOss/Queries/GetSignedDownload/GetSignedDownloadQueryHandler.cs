using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.GetSignedDownload;

public class GetSignedDownloadQueryHandler : IRequestHandler<GetSignedDownloadQuery, SignedDownloadViewModel>
{
    private readonly IApsOssService _oss;

    public GetSignedDownloadQueryHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<SignedDownloadViewModel> Handle(GetSignedDownloadQuery request, CancellationToken cancellationToken)
    {
        var result = await _oss.GetSignedDownloadAsync(request.BucketKey, request.ObjectKey, request.MinutesExpiration, cancellationToken);
        return result.ToViewModel();
    }
}
