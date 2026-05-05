using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps.Models;

namespace DesignAutomation.API.Features.ApsOss.Mapping;

public static class BucketMapper
{
    public static BucketViewModel ToViewModel(this BucketDetailsResponse source) => new()
    {
        BucketKey = source.BucketKey,
        BucketOwner = source.BucketOwner,
        CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(source.CreatedDate).UtcDateTime,
        PolicyKey = source.PolicyKey,
    };

    public static BucketListViewModel ToViewModel(this BucketListResponse source) => new()
    {
        Items = source.Items.Select(ToViewModel).ToList(),
        Next = source.Next,
    };
}
