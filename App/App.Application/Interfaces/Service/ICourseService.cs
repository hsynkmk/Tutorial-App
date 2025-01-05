using App.Application.Common;
using App.Application.DTOs.Course;
using App.Domain.Entities;

namespace App.Application.Interfaces.Service;

public interface ICourseService
{
    Task<PaginationResponse<CourseDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<PaginationResponse<CourseDto>> GetCoursesByCreatorAsync(string userId, int pageNumber, int pageSize);
    Task<CourseDto?> GetByIdAsync(int id);
    Task CreateAsync(CourseDto courseDto);
    Task UpdateAsync(CourseDto courseDto);
    Task<bool> DeleteAsync(int id);
}
