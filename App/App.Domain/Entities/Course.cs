namespace App.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Category { get; set; }
    public decimal Price { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
