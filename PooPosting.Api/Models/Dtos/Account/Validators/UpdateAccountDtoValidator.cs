// using FluentValidation;
//
// namespace PicturesAPI.Models.Dtos.Account.Validators;
//
// public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
// {
//     public UpdateAccountDtoValidator()
//     {
//         RuleFor(x => x)
//             .Must(value =>
//                 !string.IsNullOrEmpty(value.Email) ||
//                 !string.IsNullOrEmpty(value.Password) ||
//                 !string.IsNullOrEmpty(value.Description) ||
//                 value.BackgroundPic is not null ||
//                 value.ProfilePic is not null)
//             .WithMessage("email, password, description, profile pic, or background pic cannot be empty");
//
//         RuleFor(x => x.Email)
//             .EmailAddress()
//             .MaximumLength(40);
//
//         RuleFor(x => x.Password)
//             .MinimumLength(8)
//             .MaximumLength(128);
//
//         RuleFor(x => x.Description)
//             .MaximumLength(500);
//
//         RuleFor(x => x.ConfirmPassword)
//             .Equal(e => e.Password);
//     }
// }