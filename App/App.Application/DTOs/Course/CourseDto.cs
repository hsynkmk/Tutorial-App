namespace App.Application.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public decimal Price { get; set; }
    public string? CreatedBy { get; set; }
}
