using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.ListBuckets;

public class ListBucketsQueryHandler : IRequestHandler<ListBucketsQuery, BucketListViewModel>
{
    private readonly IApsOssService _oss;

    public ListBucketsQueryHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<BucketListViewModel> Handle(ListBucketsQuery request, CancellationToken cancellationToken)
    {
        var result = await _oss.ListBucketsAsync(request.Region, request.Limit, request.StartAt, cancellationToken);
        return result.ToViewModel();
    }
}
