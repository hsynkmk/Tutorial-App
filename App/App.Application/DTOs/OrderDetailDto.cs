namespace App.Application.DTOs;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public decimal Price { get; set; }
}
