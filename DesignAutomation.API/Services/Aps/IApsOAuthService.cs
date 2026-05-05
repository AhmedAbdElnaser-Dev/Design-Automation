using DesignAutomation.API.Services.Aps.Models;

namespace DesignAutomation.API.Services.Aps;

public interface IApsOAuthService
{
    string BuildAuthorizeUrl(string state);
    Task<ThreeLeggedTokenResponse> ExchangeCodeAsync(string code, CancellationToken ct = default);
    Task<ApsUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct = default);
}
