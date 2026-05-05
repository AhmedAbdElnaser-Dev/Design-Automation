using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using DesignAutomation.API.Services.Aps.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DesignAutomation.API.Services.Aps;

public class ApsAuthService : IApsAuthService
{
    private const string CacheKey = "aps:token";

    private readonly HttpClient _http;
    private readonly ApsOptions _options;
    private readonly IMemoryCache _cache;

    public ApsAuthService(HttpClient http, IOptions<ApsOptions> options, IMemoryCache cache)
    {
        _http = http;
        _options = options.Value;
        _cache = cache;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        if (_cache.TryGetValue(CacheKey, out string? cached) && !string.IsNullOrEmpty(cached))
            return cached;

        var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));

        using var req = new HttpRequestMessage(HttpMethod.Post, "/authentication/v2/token");
        req.Headers.Authorization = new AuthenticationHeaderValue("Basic", creds);
        req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["scope"] = _options.Scopes,
        });

        using var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        var token = await resp.Content.ReadFromJsonAsync<ApsTokenResponse>(cancellationToken: ct)
                    ?? throw new InvalidOperationException("Empty APS token response.");

        var ttl = TimeSpan.FromSeconds(Math.Max(60, token.ExpiresIn - 60));
        _cache.Set(CacheKey, token.AccessToken, ttl);
        return token.AccessToken;
    }
}
