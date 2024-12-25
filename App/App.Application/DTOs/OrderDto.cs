namespace App.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
}
