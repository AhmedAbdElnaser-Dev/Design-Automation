using System.ComponentModel.DataAnnotations;

namespace DesignAutomation.API.DTOs.Users;

public class UpdateUserDto
{
    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}
