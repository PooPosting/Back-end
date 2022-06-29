﻿using FluentValidation.Results;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public static class StaticValidator
{
    public static async Task<ValidationResult> Validate(UpdateAccountDto dto) =>
        await new UpdateAccountDtoValidator().ValidateAsync(dto);

    public static async Task<ValidationResult> Validate(UpdatePictureDto dto) =>
        await new UpdatePictureDtoValidator().ValidateAsync(dto);

    public static async Task<ValidationResult> Validate(CreatePictureDto dto) =>
        await new CreatePictureDtoValidator().ValidateAsync(dto);
}