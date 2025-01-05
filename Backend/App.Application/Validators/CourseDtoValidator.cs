using App.Application.DTOs.Course;
using FluentValidation;

namespace App.Application.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(course => course.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(50).WithMessage("Course name must be less than 50 characters.");

        RuleFor(course => course.Description)
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(course => course.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(course => course.Category)
            .NotEmpty().WithMessage("Category is required.");
    }
}