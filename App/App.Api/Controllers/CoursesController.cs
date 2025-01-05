using App.Application.DTOs.Course;
using App.Application.Interfaces.Service;
using App.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = Pagination.DefaultPageNumber, int pageSize = Pagination.DefaultPageSize)
    {
        var courses = await _courseService.GetAllAsync(pageNumber, pageSize);
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        return Ok(course);
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpGet("educator")]
    public async Task<IActionResult> GetCoursesByCreator(int pageNumber = Pagination.DefaultPageNumber, int pageSize = Pagination.DefaultPageSize)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var courses = await _courseService.GetCoursesByCreatorAsync(userId, pageNumber, pageSize);

        return Ok(courses);
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseDto courseDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        courseDto.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _courseService.CreateAsync(courseDto);

        return CreatedAtAction(nameof(Get), courseDto);
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CourseDto courseDto)
    {
        courseDto.Id = id;
        if (id != courseDto.Id) return BadRequest();

        await _courseService.UpdateAsync(courseDto);
        return NoContent();
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }
}

