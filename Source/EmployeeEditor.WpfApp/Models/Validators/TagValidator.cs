using EmployeeEditor.Domain.Dtos;
using FluentValidation;

namespace EmployeeEditor.WpfApp.Models.Validators;

public class TagValidator : AbstractValidator<TagDto>, IValidator
{
    public TagValidator()
    {
        RuleFor(t => t.Name).NotEmpty().WithMessage("Pole jest wymagane.")
            .MinimumLength(1).WithMessage("Pole musi zawierać conajmniej 1 znak")
            .MaximumLength(50).WithMessage("Pole musi zawierać maksymalnie 50 znaków");

        RuleFor(t => t.Description).NotEmpty().WithMessage("Pole jest wymagane.")
            .MinimumLength(5).WithMessage("Pole musi zawierać conajmniej 1 znak")
            .MaximumLength(250).WithMessage("Pole musi zawierać maksymalnie 50 znaków");
    }
}