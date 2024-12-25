using Microsoft.AspNetCore.Identity;

namespace App.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
