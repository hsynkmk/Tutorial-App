using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<IEnumerable<CourseDto>> GetCoursesByCreatorAsync(string userId);
    Task<CourseDto?> GetByIdAsync(int id);
    Task CreateAsync(CourseDto courseDto);
    Task UpdateAsync(CourseDto courseDto);
    Task<bool> DeleteAsync(int id);
}
