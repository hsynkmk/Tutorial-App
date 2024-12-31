using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public UsersController(UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (result)
        {
            return new UserDto
            {
                Email = user.Email,
                Token = await _userService.GenerateToken(user)

            };
        }
        return Unauthorized("Invalid email or password");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var newUser = new ApplicationUser
        {
            UserName = registerDto.Email,
            FullName = registerDto.Name,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerDto.Password);
        
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem();
        }

        await _userManager.AddToRoleAsync(newUser, "Student");
        return StatusCode(201);
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
