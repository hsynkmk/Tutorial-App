namespace App.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public Course Course { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public string PaymentStatus { get; set; } = "Completed";
    public string? TransactionId { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
