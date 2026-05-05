using System.ComponentModel.DataAnnotations;

namespace DesignAutomation.API.DTOs.Users;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(6)]
    public string Password { get; set; } = default!;

    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}
