using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class CreatePictureDtoValidator : AbstractValidator<CreatePictureDto>
{
    public CreatePictureDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.Picture)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .Custom((tags, context) =>
            {
                if (tags.Count > 4) context.AddFailure("maximum tag count is 4");
                foreach (var tag in tags)
                {
                    if (tag.Length > 25) context.AddFailure("maximum tag length is 25");
                }
            });
    }
}