using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Account.Validators;

public class UpdateAccountDescriptionDtoValidator: AbstractValidator<UpdateAccountDescriptionDto>
{
    public UpdateAccountDescriptionDtoValidator()
    {
        RuleFor(d => d.Description)
            .MaximumLength(500);
    }
}