using App.Domain.Entities;

namespace App.Infrastructure.Seeders;

public interface ICourseSeeder
{
    Task Seed();
}