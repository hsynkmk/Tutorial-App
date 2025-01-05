namespace App.Application.Interfaces.Repository;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    IOrderRepository Orders { get; }
    IUserRepository Users { get; }
    Task SaveAsync();
}
