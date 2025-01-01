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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Order -> ApplicationUser relationship
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction); // Disable cascading delete

        // Configure Order -> Course relationship
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Course)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction); // Disable cascading delete
    }

}
