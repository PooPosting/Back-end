using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Picture.Validators;

public class CreatePictureDtoValidator : AbstractValidator<CreatePictureDto>
{
    private const int MaxFileSize = 4194304; // 4MB
    private const int MaxTagCount = 4;
    private const int MaxTagLength = 25;

    public CreatePictureDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.FileBase64)
            .NotEmpty()
            .WithMessage("FileBase64 is required.")
            .Must(BeAValidBase64String)
            .WithMessage("FileBase64 must be a valid Base64 string.");

        RuleFor(p => p.FileBase64.Length)
            .GreaterThan(0)
            .WithMessage("Picture size must be larger than 0 bytes.")
            .LessThan(MaxFileSize)
            .WithMessage($"Picture size must be lesser than {MaxFileSize} bytes.");
        
        RuleFor(x => x.Tags)
            .Custom(ValidateTags);
    }
    
    private bool BeAValidBase64String(string base64String)
    {
        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private void ValidateTags(string[] tags, ValidationContext<CreatePictureDto> context)
    {
        if (tags is null) return;

        if (tags.Length > MaxTagCount)
            context.AddFailure($"Maximum tag count is {MaxTagCount}");

        foreach (var tag in tags)
        {
            if (tag.Length > MaxTagLength)
                context.AddFailure($"Maximum tag length is {MaxTagLength}");
        }
    }
}