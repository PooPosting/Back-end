using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class PutAccountDtoValidator : AbstractValidator<PutAccountDto>
{
    public PutAccountDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
            
        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);
    }
}