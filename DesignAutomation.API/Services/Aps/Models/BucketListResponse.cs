using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class BucketListResponse
{
    [JsonPropertyName("items")] public List<BucketDetailsResponse> Items { get; set; } = new();
    [JsonPropertyName("next")] public string? Next { get; set; }
}
