﻿using FluentValidation;

namespace PooPosting.Application.Models.Dtos.Account.Validators;

public class UpdateAccountDescriptionDtoValidator: AbstractValidator<UpdateAccountDescriptionDto>
{
    public UpdateAccountDescriptionDtoValidator()
    {
        RuleFor(d => d.Description)
            .MaximumLength(500);
    }
}