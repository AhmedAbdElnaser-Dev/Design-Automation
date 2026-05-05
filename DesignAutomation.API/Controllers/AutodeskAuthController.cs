using DesignAutomation.API.Common.OAuthState;
using DesignAutomation.API.Features.Auth.Commands.AutodeskSignIn;
using DesignAutomation.API.Services.Aps;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DesignAutomation.API.Controllers;

[ApiController]
[Route("api/auth/autodesk")]
[AllowAnonymous]
public class AutodeskAuthController : ControllerBase
{
    private readonly IApsOAuthService _oauth;
    private readonly IOAuthStateService _state;
    private readonly IMediator _mediator;
    private readonly ApsOptions _options;
    private readonly ILogger<AutodeskAuthController> _logger;

    public AutodeskAuthController(
        IApsOAuthService oauth,
        IOAuthStateService state,
        IMediator mediator,
        IOptions<ApsOptions> options,
        ILogger<AutodeskAuthController> logger)
    {
        _oauth = oauth;
        _state = state;
        _mediator = mediator;
        _options = options.Value;
        _logger = logger;
    }

    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? redirect)
    {
        var safeRedirect = IsSafeLocalPath(redirect) ? redirect : null;
        var stateToken = _state.IssueState(Response, safeRedirect);
        var url = _oauth.BuildAuthorizeUrl(stateToken);
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        [FromQuery(Name = "error_description")] string? errorDescription,
        CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(error))
            return Redirect(BuildUiCallback(error: errorDescription ?? error));

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            return Redirect(BuildUiCallback(error: "Missing authorization code or state."));

        var payload = _state.ConsumeAndValidate(Request, Response, state);
        if (payload is null)
            return Redirect(BuildUiCallback(error: "Invalid or expired sign-in attempt. Please try again."));

        try
        {
            var auth = await _mediator.Send(new AutodeskSignInCommand(code), ct);
            return Redirect(BuildUiCallback(
                token: auth.Token,
                expiresAt: auth.ExpiresAt,
                redirect: payload.RedirectPath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Autodesk sign-in failed");
            return Redirect(BuildUiCallback(error: "Sign-in failed. Please try again."));
        }
    }

    private string BuildUiCallback(string? token = null, DateTime? expiresAt = null, string? redirect = null, string? error = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrEmpty(token)) query.Add($"token={Uri.EscapeDataString(token)}");
        if (expiresAt.HasValue) query.Add($"expiresAt={Uri.EscapeDataString(expiresAt.Value.ToString("O"))}");
        if (!string.IsNullOrEmpty(redirect)) query.Add($"redirect={Uri.EscapeDataString(redirect)}");
        if (!string.IsNullOrEmpty(error)) query.Add($"error={Uri.EscapeDataString(error)}");
        return _options.UiCallbackUrl + (query.Count > 0 ? "?" + string.Join("&", query) : string.Empty);
    }

    private static bool IsSafeLocalPath(string? path)
        => !string.IsNullOrEmpty(path)
           && path.StartsWith('/')
           && !path.StartsWith("//")
           && !path.Contains("://");
}
