using App.Application.DTOs;

namespace App.Application.Interfaces;

public interface IOrderService
{
    Task CreateAsync(OrderDto orderDto);
    Task<IEnumerable<OrderDto>> GetAllByUserIdAsync(string userId);
    Task<OrderDto?> GetByIdAndUserIdAsync(int id, string userId);
}