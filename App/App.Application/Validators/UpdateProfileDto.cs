using App.Application.DTOs.Identity;
using FluentValidation;

namespace App.Application.Validators;
public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("CurrentPassword is required.");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("NewPassword is required.")
            .MinimumLength(6).WithMessage("NewPassword must be at least 6 characters.");
    }
}