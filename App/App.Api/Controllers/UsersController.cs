using App.Application.Common;
using App.Application.DTOs.Identity;
using App.Application.Interfaces.Service;
using App.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
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

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _userService.UpdateProfileAsync(userId, updateProfileDto.Name, updateProfileDto.CurrentPassword, updateProfileDto.NewPassword);

        if (!success) return BadRequest("Failed to update profile");
        return Ok("Profile updated successfully");
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<UserDto>>> GetAllUsers(int pageNumber = Pagination.DefaultPageNumber, int pageSize = Pagination.DefaultPageSize)
    {
        var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
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

    [Authorize(Roles = UserRoles.Educator)]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto updateUserDto)
    {
        if (userId != updateUserDto.Id)
            return BadRequest("User ID mismatch");

        try
        {
            var success = await _userService.UpdateUserAsync(updateUserDto);
            if (success)
                return Ok(new { message = "User updated successfully" });
            return BadRequest(new { message = "Failed to update user" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID cannot be empty");
        }

        try
        {
            var success = await _userService.DeleteUserAsync(userId);
            if (success)
            {
                return Ok(new { message = "User deleted successfully" });
            }
            return NotFound(new { message = "User not found" });
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userDto = await _userService.GetCurrentUserAsync(userId);
        return Ok(userDto);
    }
}