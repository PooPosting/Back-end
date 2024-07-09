using FluentValidation;

namespace PooPosting.Service.Models.Dtos.Account.Validators;

public class UpdateAccountEmailDtoValidator: AbstractValidator<UpdateAccountEmailDto>
{
    public UpdateAccountEmailDtoValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .MaximumLength(40)
            .EmailAddress();
    }
}