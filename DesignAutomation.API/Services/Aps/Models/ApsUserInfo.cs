using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps.Models;

public class ApsUserInfo
{
    [JsonPropertyName("sub")] public string Sub { get; set; } = default!;
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("given_name")] public string? GivenName { get; set; }
    [JsonPropertyName("family_name")] public string? FamilyName { get; set; }
    [JsonPropertyName("preferred_username")] public string? PreferredUsername { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("email_verified")] public bool EmailVerified { get; set; }
    [JsonPropertyName("picture")] public string? Picture { get; set; }
    [JsonPropertyName("locale")] public string? Locale { get; set; }
}
