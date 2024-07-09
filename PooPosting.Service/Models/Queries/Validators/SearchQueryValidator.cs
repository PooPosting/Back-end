using FluentValidation;

namespace PooPosting.Service.Models.Queries.Validators;

public class SearchQueryValidator: AbstractValidator<PictureQueryParams>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 4, 5, 6, 7, 8, 10, 15, 20 };

    public SearchQueryValidator()
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
    }
}