using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.GetBucket;

public class GetBucketQueryHandler : IRequestHandler<GetBucketQuery, BucketViewModel>
{
    private readonly IApsOssService _oss;

    public GetBucketQueryHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<BucketViewModel> Handle(GetBucketQuery request, CancellationToken cancellationToken)
    {
        var result = await _oss.GetBucketDetailsAsync(request.BucketKey, cancellationToken);
        return result.ToViewModel();
    }
}
