using App.Application.DTOs;
using App.Application.Interfaces;
using App.Application.Services;
using App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _userService = userService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var (success, token) = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
        if (!success) return Unauthorized("Invalid email or password");
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var (success, error) = await _userService.RegisterAsync(registerDto.Name, registerDto.Email, registerDto.Password);
        if (!success) return BadRequest(error);
        return Ok("Registration successful");
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateUserDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var success = await _userService.UpdateProfileAsync(userId, updateUserDto.FullName, updateUserDto.CurrentPassword, updateUserDto.NewPassword);
        if (!success) return BadRequest("Failed to update profile");
        return Ok("Profile updated successfully");
    }

    [Authorize(Roles = "Educator")]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut("{userId}/role")]
    public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] string newRole)
    {
        if (string.IsNullOrEmpty(newRole))
            return BadRequest("Role cannot be null or empty");

        try
        {
            await _userService.UpdateUserRoleAsync(userId, newRole);
            return Ok(new { message = "User role updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        return new UserDto
        {
            Email = user.Email,
            Token = await _userService.GenerateToken(user),
        };
    }

}
