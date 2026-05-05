using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class BucketPermission
{
    [JsonPropertyName("authId")] public string? AuthId { get; set; }
    [JsonPropertyName("access")] public string? Access { get; set; }
}
