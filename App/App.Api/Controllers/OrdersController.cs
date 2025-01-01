using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public OrdersController(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<Order>>> GetUserOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not authenticated");
        }

        var orders = await _unitOfWork.Orders.GetAllAsync(
            o => o.User.Id == userId,
            includeProperties: "Course,User,OrderDetails"
        );
        return Ok(orders);
    }

    [Authorize(Roles = "Educator")]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync(
            includeProperties: "User,Course,OrderDetails"
        );
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("User not authenticated");
        }

        // Retrieve user from the database using the user service
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var hasPurchased = await _unitOfWork.Orders.GetAsync(
            o => o.User.Id == userId && o.Course.Id == request.CourseId
        );

        if (hasPurchased != null)
        {
            return BadRequest("You have already purchased this course");
        }

        var course = await _unitOfWork.Courses.GetAsync(c => c.Id == request.CourseId);
        if (course == null)
        {
            return NotFound("Course not found");
        }

        var order = new Order
        {
            User = user, // Use the existing user object from the service
            Course = course,
            Price = course.Price,
            TransactionId = request.TransactionId,
            PaymentStatus = request.PaymentStatus ?? "Completed",
            OrderDetails = request.OrderDetails?.Select(od => new OrderDetail
            {

                Price = od.Quantity

            }).ToList()
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveAsync();

        return CreatedAtAction(nameof(GetUserOrders), new { }, order);
    }
}