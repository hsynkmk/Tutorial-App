using App.Domain.Entities;
using App.Infrastructure.Persistence;

namespace App.Infrastructure.Seeders;

internal class CourseSeeder(AppDbContext dbContext) : ICourseSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync() && !dbContext.Courses.Any())
        {
            var courses = GetCourses();
            await dbContext.Courses.AddRangeAsync(courses);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<Course> GetCourses()
    {
        return
        [
            new() { Name = "Course 1", Description = "Description 1", Category = 1, Price = 12 },
            new() { Name = "Course 2", Description = "Description 2", Category = 2, Price = 15 },
            new() { Name = "Course 3", Description = "Description 3", Category = 2, Price = 18 }
        ];
    }
}
