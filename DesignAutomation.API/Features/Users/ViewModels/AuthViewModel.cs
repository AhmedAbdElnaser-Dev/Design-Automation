namespace DesignAutomation.API.Features.Users.ViewModels;

public class AuthViewModel
{
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public UserViewModel User { get; set; } = default!;
}
