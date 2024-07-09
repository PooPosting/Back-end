using FluentValidation;
using PooPosting.Domain.DbContext;

namespace PooPosting.Application.Models.Dtos.Account.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountDtoValidator(PictureDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .EmailAddress();
            
        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);

        RuleFor(x => x.Nickname)
            .Custom(
                (value, context) =>
                {
                    var nicknameInUse = dbContext.Accounts.Any(a => a.Nickname == value);
                    if (nicknameInUse)
                    {
                        context.AddFailure("Nickname", "That nickname is taken");
                    }
                });
    }
}