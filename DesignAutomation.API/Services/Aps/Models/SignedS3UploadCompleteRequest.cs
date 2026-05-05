using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class SignedS3UploadCompleteRequest
{
    [JsonPropertyName("uploadKey")] public string UploadKey { get; set; } = default!;
    [JsonPropertyName("size")] public long? Size { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
}
