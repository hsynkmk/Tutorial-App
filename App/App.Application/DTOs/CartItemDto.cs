namespace App.Application.DTOs;
public class CartItemDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime AddedDate { get; set; }
}