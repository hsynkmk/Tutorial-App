using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface IUserService
{
    Task<bool> DeleteUserAsync(string userId);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetCurrentUserAsync(string userId);
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<(bool success, string token)> LoginAsync(string email, string password);
    Task<(bool success, string error)> RegisterAsync(string name, string email, string password);
    Task<bool> UpdateProfileAsync(string userId, string fullName, string currentPassword, string newPassword);
    Task<bool> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task UpdateUserRoleAsync(string userId, string newRole);
}
