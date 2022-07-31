using FluentValidation;

namespace PicturesAPI.Models.Dtos.Account.Validators;

public class LsLoginDtoValidator: AbstractValidator<LsLoginDto>
{
    public LsLoginDtoValidator()
    {
        RuleFor(x => x.Uid)
            .NotEmpty();

        RuleFor(x => x.JwtToken)
            .NotEmpty();
    }
}