﻿using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Picture.Validators;

public class CreatePictureDtoValidator : AbstractValidator<CreatePictureDto>
{
    private const int MaxFileSize = 4194304;

    public CreatePictureDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.FileBase64)
            .Must(p => p?.Length is > 0 and < MaxFileSize)
            .WithMessage($"Picture size must be larger than 0 bytes and lesser than {MaxFileSize} bytes.");

        RuleFor(x => x.Tags)
            .Custom((tags, context) =>
            {
                if (tags is null) return;

                if (tags.Length > 4) context.AddFailure("maximum tag count is 4");
                foreach (var tag in tags)
                {
                    if (tag.Length > 25) context.AddFailure("maximum tag length is 25");
                }
            });
    }
}