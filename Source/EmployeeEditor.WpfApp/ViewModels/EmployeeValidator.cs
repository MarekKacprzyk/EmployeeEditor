using EmployeeEditor.Domain.Dtos;
using FluentValidation;

namespace EmployeeEditor.WpfApp.ViewModels;

public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        const int minPhoneNumberLength = 3;
        const int maxPhoneNumberLength = 12;
        ValidName(RuleFor(x => x.Name));
        ValidName(RuleFor(x => x.Surename));
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Pole Email jest wymagane.")
            .Matches(@"^[\w\.-]+@[\w\.-]+\.\w+$").WithMessage("Niepoprawny format adresu e-mail.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
            .Must(s => ulong.TryParse(s, out var number) && s.Length is >= minPhoneNumberLength and <= maxPhoneNumberLength)
            .WithMessage($"Numer telefonu musi być liczbą całkowitą większą lub równą {minPhoneNumberLength} i mniejszą lub równą {maxPhoneNumberLength}.");
    }

    private static void ValidName(IRuleBuilder<EmployeeDto, string> nameRule)
    {
        nameRule.NotEmpty().WithMessage("Pole jest wymagane.")
            .MinimumLength(3).WithMessage("Pole musi zawierać conajmniej 3 znaki")
            .MaximumLength(50).WithMessage("Pole musi zawierać maksymalnie 50 znakó");
    }
}