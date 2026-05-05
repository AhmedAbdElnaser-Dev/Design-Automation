using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using DesignAutomation.API.Services.Aps.Models;
using Microsoft.Extensions.Options;

namespace DesignAutomation.API.Services.Aps;

public class ApsOAuthService : IApsOAuthService
{
    private const string UserInfoUrl = "https://api.userprofile.autodesk.com/userinfo";

    private readonly HttpClient _http;
    private readonly ApsOptions _options;

    public ApsOAuthService(HttpClient http, IOptions<ApsOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public string BuildAuthorizeUrl(string state)
    {
        var query = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = _options.ClientId,
            ["redirect_uri"] = _options.CallbackUrl,
            ["scope"] = _options.ThreeLeggedScopes,
            ["state"] = state,
            ["prompt"] = "login",
        };
        var qs = string.Join("&", query.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
        return $"{_options.BaseUrl}/authentication/v2/authorize?{qs}";
    }

    public async Task<ThreeLeggedTokenResponse> ExchangeCodeAsync(string code, CancellationToken ct = default)
    {
        var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));

        using var req = new HttpRequestMessage(HttpMethod.Post, "/authentication/v2/token");
        req.Headers.Authorization = new AuthenticationHeaderValue("Basic", creds);
        req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _options.CallbackUrl,
        });

        using var resp = await _http.SendAsync(req, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"APS code exchange failed ({(int)resp.StatusCode}): {body}");
        }

        return await resp.Content.ReadFromJsonAsync<ThreeLeggedTokenResponse>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty token response.");
    }

    public async Task<ApsUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, UserInfoUrl);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var resp = await _http.SendAsync(req, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"APS userinfo failed ({(int)resp.StatusCode}): {body}");
        }

        return await resp.Content.ReadFromJsonAsync<ApsUserInfo>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Empty userinfo response.");
    }
}
