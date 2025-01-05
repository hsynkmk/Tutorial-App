namespace App.Application.DTOs.Order;

public class OrderDto
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderDetailDto> OrderDetails { get; set; } = [];
}
