namespace WeightTracker.Api.ErrorDefinitions;

public sealed class NotFoundError(string message) : ErrorBase(message);
