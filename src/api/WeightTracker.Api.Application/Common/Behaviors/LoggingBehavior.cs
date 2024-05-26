using MediatR;
using Microsoft.Extensions.Logging;

namespace WeightTracker.Api.Application.Common.Behaviors;

/// <summary>
/// Represents a logging behavior.
/// </summary>
/// <remarks>
/// The logging behavior logs the request and response of the request handler.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);

        var response = await next();

        logger.LogInformation("Handled {RequestName}: {@Request} | {@Response}", requestName, request, response);

        return response;
    }
}
