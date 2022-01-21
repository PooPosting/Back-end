using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class PutAccountDtoValidator : AbstractValidator<PutAccountDto>
{
    public PutAccountDtoValidator()
    {
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