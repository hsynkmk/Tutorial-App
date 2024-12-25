using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Course> Courses { get; set; }
    public required DbSet<Order> Orders { get; set; }
    public required DbSet<OrderDetail> OrderDetails { get; set; }
    public required DbSet<CartItem> CartItems { get; set; }
    public required DbSet<ApplicationUser> Users { get; set; }

}
