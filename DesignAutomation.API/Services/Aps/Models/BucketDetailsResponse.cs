using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class BucketDetailsResponse
{
    [JsonPropertyName("bucketKey")] public string BucketKey { get; set; } = default!;
    [JsonPropertyName("bucketOwner")] public string? BucketOwner { get; set; }
    [JsonPropertyName("createdDate")] public long CreatedDate { get; set; }
    [JsonPropertyName("policyKey")] public string? PolicyKey { get; set; }
    [JsonPropertyName("permissions")] public List<BucketPermission>? Permissions { get; set; }
}
