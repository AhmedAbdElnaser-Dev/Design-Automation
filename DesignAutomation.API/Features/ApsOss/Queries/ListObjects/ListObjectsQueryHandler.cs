using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Queries.ListObjects;

public class ListObjectsQueryHandler : IRequestHandler<ListObjectsQuery, ObjectListViewModel>
{
    private readonly IApsOssService _oss;

    public ListObjectsQueryHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<ObjectListViewModel> Handle(ListObjectsQuery request, CancellationToken cancellationToken)
    {
        var result = await _oss.ListObjectsAsync(request.BucketKey, request.Limit, request.StartAt, cancellationToken);
        return result.ToViewModel();
    }
}
