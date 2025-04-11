using FluentValidation;

namespace WeightTracker.Api.Endpoints.Weight.Post;

public sealed record WeightPostRequest(decimal Weight, string Date);

public sealed class WeightPostRequestValidator : Validator<WeightPostRequest>
{
    public WeightPostRequestValidator()
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
