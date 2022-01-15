using System.Linq;
using FluentValidation;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

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