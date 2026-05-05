using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class SignedS3DownloadResponse
{
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("sha1")] public string? Sha1 { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
    [JsonPropertyName("expiration")] public long Expiration { get; set; }
}
