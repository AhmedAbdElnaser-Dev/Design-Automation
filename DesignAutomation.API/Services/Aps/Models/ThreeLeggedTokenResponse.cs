using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class ThreeLeggedTokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = default!;
    [JsonPropertyName("refresh_token")] public string? RefreshToken { get; set; }
    [JsonPropertyName("id_token")] public string? IdToken { get; set; }
    [JsonPropertyName("token_type")] public string TokenType { get; set; } = "Bearer";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
}
