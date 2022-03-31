using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class LsLoginDtoValidator: AbstractValidator<LsLoginDto>
{
    public LsLoginDtoValidator()
    {
        RuleFor(x => x.Guid).NotEmpty();
        RuleFor(x => x.JwtToken).NotEmpty();
    }
}