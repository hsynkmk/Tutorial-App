using App.Application.DTOs.Identity;
using FluentValidation;

namespace App.Application.Validators;
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email format.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}