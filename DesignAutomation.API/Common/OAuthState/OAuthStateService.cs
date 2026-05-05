using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace DesignAutomation.API.Common.OAuthState;

public class OAuthStateService : IOAuthStateService
{
    private const string CookieName = "aps_oauth_state";
    private const string CookiePath = "/api/auth/autodesk";
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(10);

    private readonly IDataProtector _protector;

    public OAuthStateService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("DesignAutomation.Aps.OAuthState.v1");
    }

    public string IssueState(HttpResponse response, string? redirectPath)
    {
        var state = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace('+', '-').Replace('/', '_').TrimEnd('=');

        var payload = new OAuthStatePayload(state, redirectPath, DateTime.UtcNow.Add(Lifetime));
        var json = JsonSerializer.Serialize(payload);
        var encrypted = _protector.Protect(json);

        response.Cookies.Append(CookieName, encrypted, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = Lifetime,
            Path = CookiePath,
            IsEssential = true,
        });

        return state;
    }

    public OAuthStatePayload? ConsumeAndValidate(HttpRequest request, HttpResponse response, string stateFromCallback)
    {
        if (!request.Cookies.TryGetValue(CookieName, out var encrypted) || string.IsNullOrEmpty(encrypted))
            return null;

        response.Cookies.Delete(CookieName, new CookieOptions { Path = CookiePath, Secure = true, SameSite = SameSiteMode.Lax });

        try
        {
            var json = _protector.Unprotect(encrypted);
            var payload = JsonSerializer.Deserialize<OAuthStatePayload>(json);
            if (payload is null) return null;
            if (payload.ExpiresAtUtc < DateTime.UtcNow) return null;

            var expected = Encoding.UTF8.GetBytes(payload.State);
            var actual = Encoding.UTF8.GetBytes(stateFromCallback);
            if (expected.Length != actual.Length || !CryptographicOperations.FixedTimeEquals(expected, actual))
                return null;

            return payload;
        }
        catch
        {
            return null;
        }
    }
}
