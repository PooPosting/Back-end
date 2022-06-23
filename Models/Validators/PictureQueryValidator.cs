using FluentValidation;

namespace PicturesAPI.Models.Validators;

public class PictureQueryValidator : AbstractValidator<PictureQuery>
{    
    private readonly int[] _allowedPageSizes = { 2, 3, 5, 7 };

    public PictureQueryValidator()
    {
        RuleFor(p => p.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
            }
        });
    }
}