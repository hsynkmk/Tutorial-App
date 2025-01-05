using App.Domain.Entities;
using App.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Seeders;

internal class UserSeeder(AppDbContext dbContext, UserManager<ApplicationUser> userManager) : IUserSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                await dbContext.Roles.AddRangeAsync(roles);
                await dbContext.SaveChangesAsync();
            }


            if (!userManager.Users.Any())
            {
                var users = GetUsers();
                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user.Item1, user.Item2);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user.Item1, user.Item3);
                    }
                }
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        return
        [
            new() {
                Name = "Educator",
                NormalizedName = "EDUCATOR"
            },
            new() {
                Name = "Student",
                NormalizedName = "STUDENT"
            }
        ];
    }

    private IEnumerable<(ApplicationUser, string, string)> GetUsers()
    {
        return new List<(ApplicationUser, string, string)>
        {
            (new ApplicationUser
            {
                FullName = "Main Educator",
                UserName = "educator",
                Email = "educator@gmail.com",
                EmailConfirmed = true
            }, "Educator1.", "Educator"),

            (new ApplicationUser
            {
                FullName = "Main Student",
                UserName = "student",
                Email = "student@gmail.com",
                EmailConfirmed = true
            }, "Student1.", "Student"),
        };
    }
}