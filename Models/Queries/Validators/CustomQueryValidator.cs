using FluentValidation;

namespace PicturesAPI.Models.Queries.Validators;

public class CustomQueryValidator: AbstractValidator<CustomQuery>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 5, 7, 10 };

    public CustomQueryValidator()
    {
        RuleFor(p => p.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
            }
        });
        RuleFor(p => p.PageNumber)
            .NotEmpty()
            .Must(pn => pn > 0);
        RuleFor(p => p.SearchPhrase)
            .NotEmpty();
    }
}