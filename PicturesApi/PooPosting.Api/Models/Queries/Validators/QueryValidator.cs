using FluentValidation;

namespace PooPosting.Api.Models.Queries.Validators;

public class QueryValidator: AbstractValidator<Query>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 5, 7, 10 };

    public QueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .Must(q => q > 0)
            .WithMessage("PageNumber must be greater than 0");

        RuleFor(q => q.PageSize)
            .Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
            }
        });
    }
}