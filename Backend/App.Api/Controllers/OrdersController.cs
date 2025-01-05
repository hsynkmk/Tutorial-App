using App.Application.DTOs.Order;
using App.Application.Interfaces.Repository;
using App.Application.Interfaces.Service;
using App.Domain.Common;
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

        var orders = await _orderService.GetUserOrdersAsync(userId);
        
        return Ok(orders);
    }

    [Authorize(Roles = UserRoles.Educator)]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var order = await _orderService.CreateOrderAsync(request, userId);

        return CreatedAtAction(nameof(GetUserOrders), new { }, order);
    }
}