using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class PutAccountDtoValidator : AbstractValidator<PutAccountDto>
{
    public PutAccountDtoValidator()
    {
        RuleFor(x => x)
            .Must(value =>
                !string.IsNullOrEmpty(value.Email) ||
                !string.IsNullOrEmpty(value.Password))
            .WithMessage("Email or password cannot be empty");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(40);

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .MaximumLength(128);
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);
    }
}