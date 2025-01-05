using App.Application.DTOs.Order;
using App.Application.Interfaces.Repository;
using App.Application.Interfaces.Service;
using App.Domain.Common;
using App.Domain.Entities;
using App.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace App.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public OrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userService = userService;
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

    public async Task<Order> CreateOrderAsync(CreateOrderDto request, string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) throw new FailedOperationException("User not found");


        var hasPurchased = await _unitOfWork.Orders.GetAsync(
            o => o.User.Id == userId && o.Course.Id == request.CourseId
        );
        if (hasPurchased != null) throw new FailedOperationException("You have already purchased this course");

        var course = await _unitOfWork.Courses.GetAsync(c => c.Id == request.CourseId);
        if (course == null) throw new FailedOperationException("Course not found");

        var order = new Order
        {
            User = user,
            Course = course,
            Price = course.Price,
            TransactionId = request.TransactionId,
            PaymentStatus = request.PaymentStatus ?? Const.Completed,
            OrderDetails = request.OrderDetails?.Select(od => new OrderDetail { Price = od.Quantity }).ToList()
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
