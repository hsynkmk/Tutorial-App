using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<List<Order>> GetUserOrdersAsync(string userId)
    {
        return (await _unitOfWork.Orders.GetAllAsync(
            o => o.User.Id == userId,
            includeProperties: "Course,User,OrderDetails"
        )).OrderByDescending(o => o.PurchaseDate).ToList();
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return (await _unitOfWork.Orders.GetAllAsync(
            includeProperties: "User,Course,OrderDetails"
        )).OrderByDescending(o => o.PurchaseDate).ToList();
    }

    public async Task<Order> CreateOrderAsync(string userId, int courseId, string? paymentStatus, string? transactionId, List<CreateOrderDetailRequest>? orderDetails)
    {
        var course = await _unitOfWork.Courses.GetAsync(c => c.Id == courseId);
        if (course == null) throw new Exception("Course not found");

        // Retrieve the user directly from the unit of work
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        var order = new Order
        {
            User = user, // Set the existing user
            Course = course,
            Price = course.Price,
            PaymentStatus = paymentStatus ?? "Completed",
            TransactionId = transactionId,
            OrderDetails = orderDetails?.Select(od => new OrderDetail
            {

                Price = od.Quantity

            }).ToList()
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveAsync();

        return order;
    }

    public async Task<bool> HasUserPurchasedCourseAsync(string userId, int courseId)
    {
        var order = await _unitOfWork.Orders.GetAsync(o => o.User.Id == userId && o.Course.Id == courseId);
        return order != null;
    }

    public async Task<List<Course>> GetUserPurchasedCoursesAsync(string userId)
    {
        return (await _unitOfWork.Orders.GetAllAsync(
            o => o.User.Id == userId,
            includeProperties: "Course"
        )).Select(o => o.Course).Distinct().ToList();
    }
}
