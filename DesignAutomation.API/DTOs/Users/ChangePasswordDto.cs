using System.ComponentModel.DataAnnotations;

namespace DesignAutomation.API.DTOs.Users;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = default!;

    [Required, MinLength(6)]
    public string NewPassword { get; set; } = default!;
}
