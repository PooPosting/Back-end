using System.Data;
using FluentValidation;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Models.Validators;

public class PatchRestrictedIpValidator: AbstractValidator<PatchRestrictedIp>
{
    public PatchRestrictedIpValidator()
    {
        RuleFor(p => p.Ips).NotEmpty();
    }
}