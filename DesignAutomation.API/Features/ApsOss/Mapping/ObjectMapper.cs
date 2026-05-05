using DesignAutomation.API.Features.ApsOss.ViewModels;
using DesignAutomation.API.Services.Aps.Models;

namespace DesignAutomation.API.Features.ApsOss.Mapping;

public static class ObjectMapper
{
    public static ObjectViewModel ToViewModel(this ObjectDetailsResponse source) => new()
    {
        BucketKey = source.BucketKey,
        ObjectKey = source.ObjectKey,
        ObjectId = source.ObjectId,
        Size = source.Size,
        ContentType = source.ContentType,
        Location = source.Location,
    };

    public static ObjectListViewModel ToViewModel(this ObjectListResponse source) => new()
    {
        Items = source.Items.Select(ToViewModel).ToList(),
        Next = source.Next,
    };

    public static SignedDownloadViewModel ToViewModel(this SignedS3DownloadResponse source) => new()
    {
        Status = source.Status,
        Url = source.Url,
        Size = source.Size,
        Sha1 = source.Sha1,
        ContentType = source.ContentType,
        ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(source.Expiration).UtcDateTime,
    };
}
