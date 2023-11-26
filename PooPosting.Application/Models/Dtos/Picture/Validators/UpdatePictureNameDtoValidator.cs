using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Picture.Validators;

public class UpdatePictureNameDtoValidator: AbstractValidator<UpdatePictureNameDto>
{
    public UpdatePictureNameDtoValidator()
    {
        RuleFor(n => n.Name)
            .MinimumLength(4)
            .MaximumLength(40);
    }
}