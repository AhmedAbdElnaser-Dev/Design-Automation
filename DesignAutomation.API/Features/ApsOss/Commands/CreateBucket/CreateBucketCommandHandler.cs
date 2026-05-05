using DesignAutomation.API.Features.ApsOss.Mapping;
using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps;
using MediatR;

namespace DesignAutomation.API.Features.ApsOss.Commands.CreateBucket;

public class CreateBucketCommandHandler : IRequestHandler<CreateBucketCommand, BucketViewModel>
{
    private readonly IApsOssService _oss;

    public CreateBucketCommandHandler(IApsOssService oss)
    {
        _oss = oss;
    }

    public async Task<BucketViewModel> Handle(CreateBucketCommand request, CancellationToken cancellationToken)
    {
        var result = await _oss.CreateBucketAsync(request.BucketKey, request.PolicyKey, request.Region, cancellationToken);
        return result.ToViewModel();
    }
}
