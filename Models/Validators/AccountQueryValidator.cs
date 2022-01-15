using System.Linq;
using FluentValidation;

namespace PicturesAPI.Models.Validators;

public class AccountQueryValidator : AbstractValidator<AccountQuery>
{
    private readonly int[] _allowedPageSizes = new[] { 3, 5, 10 };

    public AccountQueryValidator()
    {

        RuleFor(p => p.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(p => p.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
            }
        });
    }
}