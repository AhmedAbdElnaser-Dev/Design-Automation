namespace DesignAutomation.API.Services.Aps;

public class ApsOptions
{
    public const string SectionName = "Aps";

    public string BaseUrl { get; set; } = "https://developer.api.autodesk.com";
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string Scopes { get; set; } = "data:read data:write bucket:read bucket:create";
    public string DefaultBucketPolicy { get; set; } = "transient";

    public string ThreeLeggedScopes { get; set; } = "user-profile:read openid";
    public string CallbackUrl { get; set; } = default!;
    public string UiCallbackUrl { get; set; } = default!;
}
