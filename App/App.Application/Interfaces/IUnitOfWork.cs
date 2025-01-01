namespace App.Application.Interfaces;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    IOrderRepository Orders { get; }
    IUserRepository Users { get; }
    Task SaveAsync();
}
