namespace WeightTracker.Api.ErrorDefinitions;

public sealed class InternalError(string message) : ErrorBase(message);
