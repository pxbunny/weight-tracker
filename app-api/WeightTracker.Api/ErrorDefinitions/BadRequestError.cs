namespace WeightTracker.Api.ErrorDefinitions;

public sealed class BadRequestError(string message) : ErrorBase(message);
