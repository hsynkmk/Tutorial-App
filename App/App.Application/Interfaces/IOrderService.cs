using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest request, string userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetUserOrdersAsync(string userId);
    Task<List<Course>> GetUserPurchasedCoursesAsync(string userId);
    Task<bool> HasUserPurchasedCourseAsync(string userId, int courseId);
}