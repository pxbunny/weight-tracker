﻿using FluentValidation;

namespace WeightTracker.Api.Endpoints.Weight.Put;

public sealed record WeightPutRequest(string Date, decimal Weight);

public sealed class WeightPutRequestValidator : Validator<WeightPutRequest>
{
    public WeightPutRequestValidator()
    {
        RuleFor(r => r.Weight)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(500)
            .WithMessage("Dude, no way!");

        RuleFor(r => r.Date)
            .NotEmpty()
            .Must(date => date.IsValidDomainDateFormat())
            .WithMessage("Invalid date format");
    }
}
