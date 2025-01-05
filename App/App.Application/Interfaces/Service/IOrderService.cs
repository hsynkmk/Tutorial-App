using App.Application.DTOs.Order;
using App.Domain.Entities;

namespace App.Application.Interfaces.Service;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto request, string userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetUserOrdersAsync(string userId);
    Task<List<Course>> GetUserPurchasedCoursesAsync(string userId);
    Task<bool> HasUserPurchasedCourseAsync(string userId, int courseId);
}