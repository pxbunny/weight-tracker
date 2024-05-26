using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WeightTracker.Api.Application.Common.Behaviors;

/// <summary>
/// Represents a performance behavior.
/// </summary>
/// <remarks>
/// The performance behavior logs the request and response of the request handler if the request takes longer than 500 milliseconds.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
internal sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();

        timer.Start();

        var response = await next();

        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
        {
            return response;
        }

        var requestName = typeof(TRequest).Name;

        logger.LogWarning("Long Running Request: {RequestName} ({ElapsedMilliseconds} ms) {@Request}", requestName, elapsedMilliseconds, request);

        return response;
    }
}
