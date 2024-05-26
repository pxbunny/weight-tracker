namespace WeightTracker.Domain.Common.Response;

/// <inheritdoc />
/// <typeparam name="T">The type of the data.</typeparam>
public interface IResponse<out T> : IResponse
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>
    /// The data. This value is <see langword="null"/> if the response is not successful.
    /// </value>
    T? Data { get; }
}

/// <summary>
/// Represents a response object.
/// </summary>
/// <remarks>
/// The main purpose of this class is to provide a consistent way
/// to return responses with status from methods instead of using exceptions.
/// </remarks>
public interface IResponse
{
    /// <summary>
    /// Gets a value indicating whether the response is successful.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the response is successful; otherwise <see langword="false"/>.
    /// </value>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value>
    /// The error message. This value is <see langword="null"/> if the response is successful.
    /// </value>
    string? ErrorMessage { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    /// <value>
    /// The error code. This value is <see langword="null"/> if the response is successful.
    /// </value>
    int? ErrorCode { get; init; }
}
