using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class ApsTokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = default!;
    [JsonPropertyName("token_type")] public string TokenType { get; set; } = "Bearer";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
}
