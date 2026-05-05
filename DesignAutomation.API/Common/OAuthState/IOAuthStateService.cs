namespace DesignAutomation.API.Common.OAuthState;

public interface IOAuthStateService
{
    string IssueState(HttpResponse response, string? redirectPath);
    OAuthStatePayload? ConsumeAndValidate(HttpRequest request, HttpResponse response, string stateFromCallback);
}
