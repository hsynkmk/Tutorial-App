using App.Application.DTOs.Identity;
using FluentValidation;

namespace App.Application.Validators;
public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email format.");
    }
}