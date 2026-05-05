using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class ObjectDetailsResponse
{
    [JsonPropertyName("bucketKey")] public string BucketKey { get; set; } = default!;
    [JsonPropertyName("objectKey")] public string ObjectKey { get; set; } = default!;
    [JsonPropertyName("objectId")] public string ObjectId { get; set; } = default!;
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
    [JsonPropertyName("location")] public string? Location { get; set; }
}
