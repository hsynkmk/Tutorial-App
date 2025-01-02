using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface IUserService
{
    Task<(bool success, string token)> LoginAsync(string email, string password);
    Task<(bool success, string error)> RegisterAsync(string name, string email, string password);
    Task<bool> UpdateProfileAsync(string userId, string fullName, string currentPassword, string newPassword);
    Task UpdateUserRoleAsync(string userId, string newRole);
    Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<bool> DeleteUserAsync(string userId);
    Task<string> GenerateToken(ApplicationUser user);
}
