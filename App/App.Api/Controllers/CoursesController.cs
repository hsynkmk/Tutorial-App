using App.Application.DTOs;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedCourses = await _courseService.GetAllAsync(pageNumber, pageSize);
        return Ok(paginatedCourses);
    }


    [HttpGet("educator")]
    public async Task<IActionResult> GetCoursesByCreator(int pageNumber = 1, int pageSize = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not logged in");
        }

        var courses = await _courseService.GetCoursesByCreatorAsync(userId, pageNumber, pageSize);

        return Ok(courses);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseDto courseDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        courseDto.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _courseService.CreateAsync(courseDto);
        return CreatedAtAction(nameof(Get), courseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CourseDto courseDto)
    {
        courseDto.Id = id;
        if (id != courseDto.Id) return BadRequest();

        await _courseService.UpdateAsync(courseDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }
}

