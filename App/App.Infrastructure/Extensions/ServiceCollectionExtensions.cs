using App.Application.Interfaces;
using App.Infrastructure.Persistence;
using App.Infrastructure.Seeders;
using App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        services.AddAuthentication();
        services.AddAuthorization();

        services.AddScoped<ICourseSeeder, CourseSeeder>();
        services.AddScoped<IUserSeeder, UserSeeder>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
