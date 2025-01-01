using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces;

public interface IUserService
{
    Task<(bool success, string token)> LoginAsync(string email, string password);
    Task<(bool success, string error)> RegisterAsync(string name, string email, string password);
    Task<bool> UpdateProfileAsync(string userId, string fullName, string currentPassword, string newPassword);
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<string> GenerateToken(ApplicationUser user);
}
