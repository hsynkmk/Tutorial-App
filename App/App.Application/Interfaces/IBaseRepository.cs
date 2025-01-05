using App.Domain.Entities;
using System.Linq.Expressions;

namespace App.Application.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    IQueryable<T> GetAllQueryable(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    void Remove(T entity);
    void Update(T entity);
}
