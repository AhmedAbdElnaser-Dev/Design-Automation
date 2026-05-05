using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class SignedS3UploadResponse
{
    [JsonPropertyName("uploadKey")] public string UploadKey { get; set; } = default!;
    [JsonPropertyName("uploadExpiration")] public string? UploadExpiration { get; set; }
    [JsonPropertyName("urlExpiration")] public string? UrlExpiration { get; set; }
    [JsonPropertyName("urls")] public List<string> Urls { get; set; } = new();
}
