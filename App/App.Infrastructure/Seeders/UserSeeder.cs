using App.Domain.Entities;
using App.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Seeders;

internal class UserSeeder(AppDbContext dbContext) : IUserSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync() && !dbContext.Roles.Any())
        {
            var roles = GetRoles();
            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
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
}
