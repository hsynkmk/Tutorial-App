using App.Application.DTOs;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var courses = await _courseService.GetAllAsync();
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

