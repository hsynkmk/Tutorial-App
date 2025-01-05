using App.Application.Interfaces.Repository;
using App.Domain.Entities;
using App.Infrastructure.Persistence;

namespace App.Infrastructure.Repositories;

internal class UserRepository(AppDbContext context) : BaseRepository<ApplicationUser>(context), IUserRepository
{
}
