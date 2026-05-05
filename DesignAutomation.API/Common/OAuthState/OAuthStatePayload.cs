namespace DesignAutomation.API.Common.OAuthState;

public record OAuthStatePayload(string State, string? RedirectPath, DateTime ExpiresAtUtc);
