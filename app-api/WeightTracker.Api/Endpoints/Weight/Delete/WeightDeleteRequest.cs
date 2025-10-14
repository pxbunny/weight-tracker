using FluentValidation;

namespace WeightTracker.Api.Endpoints.Weight.Delete;

public sealed record WeightDeleteRequest(string Date);

public sealed class WeightDeleteRequestValidator : Validator<WeightDeleteRequest>
{
    public WeightDeleteRequestValidator()
    {
        RuleFor(r => r.Date)
            .NotEmpty()
            .Must(date => date.IsValidDomainDateFormat())
            .WithMessage("Invalid date format");
    }
}
