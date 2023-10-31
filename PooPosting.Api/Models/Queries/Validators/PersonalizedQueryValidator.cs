using FluentValidation;

namespace PooPosting.Api.Models.Queries.Validators;

public class PersonalizedQueryValidator: AbstractValidator<PersonalizedQuery>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 4, 5, 6, 7, 8, 10, 15, 20 };

    public PersonalizedQueryValidator()
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