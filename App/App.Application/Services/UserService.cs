using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace App.Application.Services;

internal class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserService(
        UserManager<ApplicationUser> userManager,
        //SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        //_signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<(bool success, string token)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return (false, null);

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result) return (false, null);

        await _userManager.UpdateAsync(user);

        var token = await GenerateToken(user);
        return (true, token);
    }

    public async Task<(bool success, string error)> RegisterAsync(string name, string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            FullName = name,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "Student");
        return (true, null);
    }

    public async Task<bool> UpdateProfileAsync(string userId, string fullName, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.FullName = fullName;

        if (!string.IsNullOrEmpty(newPassword))
        {
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!changePasswordResult.Succeeded) return false;
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    //public async Task<bool> UpdateUserRoleAsync(string userId, string newRole)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null) return false;

    //    user.Role = newRole;
    //    var result = await _userManager.UpdateAsync(user);
    //    return result.Succeeded;
    //}

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }


    public async Task<string> GenerateToken(ApplicationUser user)
    {
        //claims
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:TokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
