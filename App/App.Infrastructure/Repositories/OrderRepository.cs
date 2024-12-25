using App.Application.Interfaces;
using App.Domain.Entities;
using App.Infrastructure.Persistence;

namespace App.Infrastructure.Repositories;

internal class OrderRepository(AppDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
}
