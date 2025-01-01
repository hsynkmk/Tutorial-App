using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.Exceptions;
using AutoMapper;
using App.Domain.Common;

namespace App.Application.Services;


internal class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<OrderDto>> GetAllByUserIdAsync(string userId)
    {
        var orders = await _unitOfWork.Orders.GetAllAsync(o => o.User.Id == userId, "Course");
        if (orders == null || !orders.Any()) throw new NotFoundException(Constans.Order, "all");

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAndUserIdAsync(int id, string userId)
    {
        var order = await _unitOfWork.Orders.GetAsync(o => o.Id == id && o.User.Id == userId, "Course");
        if (order == null) throw new NotFoundException(Constans.Order, id);

        return _mapper.Map<OrderDto?>(order);
    }

    public async Task CreateAsync(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        order.PurchaseDate = DateTime.UtcNow;

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveAsync();
    }
}
