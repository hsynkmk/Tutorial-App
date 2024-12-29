using App.Application.Interfaces;
using App.Infrastructure.Persistence;

namespace App.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICourseRepository Courses { get; private set; }

    public IOrderRepository Orders { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Courses = new CourseRepository(_context);
        Orders = new OrderRepository(_context);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
