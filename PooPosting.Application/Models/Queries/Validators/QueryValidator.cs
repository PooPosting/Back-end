using FluentValidation;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Models.Queries.Validators;

public class QueryValidator: AbstractValidator<PaginationParameters>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 4, 5, 6, 7, 8, 10, 15, 20 };

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