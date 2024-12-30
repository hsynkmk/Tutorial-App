using App.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public required DbSet<Course> Courses { get; set; }
    public required DbSet<Order> Orders { get; set; }
    public required DbSet<OrderDetail> OrderDetails { get; set; }
    public required DbSet<CartItem> CartItems { get; set; }

}
