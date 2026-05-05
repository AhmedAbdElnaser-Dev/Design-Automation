using DesignAutomation.API.Models;

namespace DesignAutomation.API.Services;

public interface ITokenService
{
    Task<(string Token, DateTime ExpiresAt)> CreateTokenAsync(ApplicationUser user);
}
