using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Picture.Validators;

public class UpdatePictureDescriptionDtoValidator: AbstractValidator<UpdatePictureDescriptionDto>
{
    public UpdatePictureDescriptionDtoValidator()
    {
        RuleFor(d => d.Description)
            .MaximumLength(500);
    }
}