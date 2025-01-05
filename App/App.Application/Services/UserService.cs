using App.Application.Common;
using App.Application.DTOs.Identity;
using App.Application.Interfaces.Service;
using App.Domain.Common;
using App.Domain.Entities;
using App.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<(bool success, string token)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return (false, null);

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result) return (false, null);

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

        await _userManager.AddToRoleAsync(user, UserRoles.Student);
        return (true, null);
    }

    public async Task<bool> UpdateProfileAsync(string userId, string fullName, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var passwordValid = await _userManager.CheckPasswordAsync(user, currentPassword);
        if (!passwordValid) return false;

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
            throw new FailedOperationException("User not found");

        user.FullName = updateUserDto.FullName;
        user.Email = updateUserDto.Email;
        user.UserName = updateUserDto.Email;

        //new password
        if (!string.IsNullOrEmpty(updateUserDto.NewPassword))
        {
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    var errorMessages = string.Join(", ", removePasswordResult.Errors.Select(e => e.Description));
                    throw new FailedOperationException($"Password removal failed: {errorMessages}");
                }
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, updateUserDto.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                var errorMessages = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
                throw new FailedOperationException($"Password addition failed: {errorMessages}");
            }
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new FailedOperationException($"User update failed: {errorMessages}");
        }

        return true;
    }


    public async Task UpdateUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new FailedOperationException("User not found");

        if (!await _roleManager.RoleExistsAsync(newRole))
            throw new FailedOperationException("Role does not exist");

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
                throw new FailedOperationException("Failed to remove existing roles");
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!addRoleResult.Succeeded)
            throw new FailedOperationException("Failed to assign new role");
    }

    public async Task<PaginationResponse<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        var totalRecords = await _userManager.Users.CountAsync();
        var users = await _userManager.Users
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role";
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = role;
            userDtos.Add(userDto);
        }

        return new PaginationResponse<UserDto>(pageNumber, pageSize, totalRecords, userDtos);
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

    public async Task<UserDto> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new FailedOperationException("User not found");

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role";
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = role;
        userDto.Token = await GenerateToken(user);
        return userDto;
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
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
