using App.Application.DTOs.Order;
using FluentValidation;

namespace App.Application.Validators;
public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate is required.");
        RuleFor(x => x.OrderDetails).NotEmpty().WithMessage("OrderDetails cannot be empty.");
    }
}