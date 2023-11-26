using FluentValidation;

namespace PooPosting.Application.Models.Dtos.Account.Validators;

public class UpdateAccountPasswordDtoValidator: AbstractValidator<UpdateAccountPasswordDto>
{
    public UpdateAccountPasswordDtoValidator()
    {
        RuleFor(p => p.Password)
            .MinimumLength(8);

        RuleFor(p => p.ConfirmPassword)
            .Equal(e => e.Password)
            .WithMessage("Passwords are not equal");
    }
}