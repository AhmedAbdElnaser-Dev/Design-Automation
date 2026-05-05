using System.ComponentModel.DataAnnotations;

namespace DesignAutomation.API.DTOs.Users;

public class LoginDto
{
    [Required]
    public string EmailOrUserName { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
