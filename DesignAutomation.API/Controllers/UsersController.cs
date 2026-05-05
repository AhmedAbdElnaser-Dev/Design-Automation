using AutoMapper;
using DesignAutomation.API.DTOs.Users;
using DesignAutomation.API.Models;
using DesignAutomation.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DesignAutomation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var user = _mapper.Map<ApplicationUser>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var (token, expiresAt) = await _tokenService.CreateTokenAsync(user);
        return Ok(new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = await ToUserDtoAsync(user),
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.EmailOrUserName)
                  ?? await _userManager.FindByNameAsync(dto.EmailOrUserName);
        if (user is null)
            return Unauthorized("Invalid credentials.");

        var check = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
        if (!check.Succeeded)
            return Unauthorized("Invalid credentials.");

        var (token, expiresAt) = await _tokenService.CreateTokenAsync(user);
        return Ok(new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = await ToUserDtoAsync(user),
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();
        return Ok(await ToUserDtoAsync(user));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserDto>(users.Count);
        foreach (var u in users)
            result.Add(await ToUserDtoAsync(u));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();
        return Ok(await ToUserDtoAsync(user));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Update(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var currentId = _userManager.GetUserId(User);
        if (currentId != id && !User.IsInRole("Admin"))
            return Forbid();

        if (dto.UserName is not null) user.UserName = dto.UserName;
        if (dto.Email is not null) user.Email = dto.Email;
        if (dto.PhoneNumber is not null) user.PhoneNumber = dto.PhoneNumber;
        if (dto.FullName is not null) user.FullName = dto.FullName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(await ToUserDtoAsync(user));
    }

    [HttpPost("{id}/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var currentId = _userManager.GetUserId(User);
        if (currentId != id) return Forbid();

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpPost("{id}/roles/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddToRole(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id}/roles/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveFromRole(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }

    private async Task<UserDto> ToUserDtoAsync(ApplicationUser user)
    {
        var dto = _mapper.Map<UserDto>(user);
        dto.Roles = await _userManager.GetRolesAsync(user);
        return dto;
    }
}
