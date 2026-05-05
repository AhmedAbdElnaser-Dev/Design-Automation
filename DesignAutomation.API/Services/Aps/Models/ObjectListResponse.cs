using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class ObjectListResponse
{
    [JsonPropertyName("items")] public List<ObjectDetailsResponse> Items { get; set; } = new();
    [JsonPropertyName("next")] public string? Next { get; set; }
}
