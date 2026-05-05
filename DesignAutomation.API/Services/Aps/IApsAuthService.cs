namespace DesignAutomation.API.Services.Aps;

public interface IApsAuthService
{
    Task<string> GetAccessTokenAsync(CancellationToken ct = default);
}
