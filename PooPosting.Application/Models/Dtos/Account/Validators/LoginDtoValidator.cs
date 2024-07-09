using FluentValidation;

namespace PooPosting.Application.Models.Dtos.Account.Validators;

public class LoginDtoValidator : AbstractValidator<LoginWithAuthCredsDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}