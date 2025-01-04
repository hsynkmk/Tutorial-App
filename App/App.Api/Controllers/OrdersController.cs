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
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    public OrdersController(IUnitOfWork unitOfWork, IUserService userService, IOrderService orderService)
    {
        _userService = userService;
        _orderService = orderService;
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<Order>>> GetUserOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not authenticated");
        }

        var orders = await _orderService.GetUserOrdersAsync(userId);
        
        return Ok(orders);
    }

    [Authorize(Roles = "Educator")]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
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

        var order = await _orderService.CreateOrderAsync(request, userId);

        return CreatedAtAction(nameof(GetUserOrders), new { }, order);
    }
}