using System.Linq;
using FluentValidation;

namespace PicturesAPI.Models.Validators;

public class PictureQueryValidator : AbstractValidator<PictureQuery>
{    
    private readonly int[] _allowedPageSizes = new[] { 10, 20, 40 };

    public PictureQueryValidator()
    {

        RuleFor(p => p.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(p => p.LikedTags)
            .NotNull();
        
        RuleFor(p => p.DaysSincePictureAdded)
            .NotNull();
        
        RuleFor(p => p.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
            }
        });
    }
}