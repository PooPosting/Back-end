using System.Data;
using FluentValidation;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models.Validators;

public class PatchRestrictedIpValidator: AbstractValidator<PatchRestrictedIp>
{
    public PatchRestrictedIpValidator()
    {
        RuleFor(p => p.Ips).NotEmpty();
    }
}