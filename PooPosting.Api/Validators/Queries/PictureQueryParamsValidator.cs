using FluentValidation;
using PooPosting.Domain.DbContext.Interfaces;

namespace PooPosting.Api.Validators.Queries;

public class PictureQueryParamsValidator: AbstractValidator<IPaginationParameters>
{
    private readonly int[] allowedPageSizes = { 2, 3, 4, 5, 6, 7, 8, 10, 15, 20 };

    public PictureQueryParamsValidator()
    {
        RuleFor(p => p.PageNumber)
            .NotEmpty()
            .Must(pn => pn > 0);
        
        RuleFor(p => p.PageSize)
            .Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
            }
        });
        
    }
}