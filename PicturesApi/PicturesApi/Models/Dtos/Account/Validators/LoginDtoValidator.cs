using FluentValidation;

namespace PicturesAPI.Models.Dtos.Account.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}