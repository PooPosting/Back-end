using System.Data;
using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class UpdatePictureDtoValidator : AbstractValidator<UpdatePictureDto>
{
    public UpdatePictureDtoValidator()
    {
        RuleFor(x => x)
            .Must(value =>
                !string.IsNullOrEmpty(value.Name) ||
                !string.IsNullOrEmpty(value.Description) ||
                (value.Tags is not null && value.Tags.Count != 0))
            .WithMessage("name, description or tags cannot be empty");

        RuleFor(x => x.Name)
            .MaximumLength(40);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Tags)
            .Custom((tags, context) =>
            {
                if (tags is null) return;

                if (tags.Count > 4) context.AddFailure("maximum tag count is 4");
                foreach (var tag in tags)
                {
                    if (tag.Length > 25) context.AddFailure("maximum tag length is 25");
                }
            });

    }
}