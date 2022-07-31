using FluentValidation;

namespace PicturesAPI.Models.Queries.Validators;

public class PersonalizedQueryValidator: AbstractValidator<PersonalizedQuery>
{
    private readonly int[] _allowedPageSizes = { 2, 3, 5 };

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