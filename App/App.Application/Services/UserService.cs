using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using AutoMapper;
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
    private readonly RoleManager<IdentityRole> _roleManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        //SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        //_signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
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

    public async Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(updateUserDto.Id);
        if (user == null)
            throw new Exception("User not found");

        // Update basic info
        user.FullName = updateUserDto.FullName;
        user.Email = updateUserDto.Email;
        user.UserName = updateUserDto.Email; // UserName is often set to email in many applications

        // Update password if provided
        if (!string.IsNullOrEmpty(updateUserDto.NewPassword))
        {
            var changePasswordResult = await _userManager.ChangePasswordAsync(
                user,
                updateUserDto.CurrentPassword,
                updateUserDto.NewPassword
            );

            if (!changePasswordResult.Succeeded)
            {
                var errorMessages = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
                throw new Exception($"Password update failed: {errorMessages}");
            }
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User update failed: {errorMessages}");
        }

        return true;
    }

    public async Task UpdateUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        if (!await _roleManager.RoleExistsAsync(newRole))
            throw new Exception("Role does not exist");

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
                throw new Exception("Failed to remove existing roles");
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!addRoleResult.Succeeded)
            throw new Exception("Failed to assign new role");
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role";
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = role;
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }



    public async Task<string> GenerateToken(ApplicationUser user)
    {
        //claims
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
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
