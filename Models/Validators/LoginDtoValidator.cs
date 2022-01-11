using FluentValidation;

namespace PicturesAPI.Models.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Nickname)
            .MinimumLength(4);

        RuleFor(x => x.Password)
            .MinimumLength(8);
    }
}