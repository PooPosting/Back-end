using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class CreatePictureDtoValidator : AbstractValidator<CreatePictureDto>
{
    public CreatePictureDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
    }
}