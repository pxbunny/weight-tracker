namespace WeightTracker.Domain.Common.Response;

/// <inheritdoc />
public sealed class Response<T> : IResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Response{T}"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor is private to prevent the creation of instances of this class.
    /// </remarks>
    private Response()
    {
    }

    /// <inheritdoc />
    public T? Data { get; init; }

    /// <inheritdoc />
    public required bool IsSuccess { get; init; }

    /// <inheritdoc />
    public string? ErrorMessage { get; init; }

    /// <inheritdoc />
    public int? ErrorCode { get; init; }

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>A successful response with the specified data.</returns>
    public static Response<T> Success(T data) => new()
    {
        Data = data,
        IsSuccess = true
    };

    /// <summary>
    /// Creates a failed response.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns>A failed response with the specified error message and error code.</returns>
    public static Response<T> Fail(string errorMessage, int errorCode) => new()
    {
        ErrorMessage = errorMessage,
        ErrorCode = errorCode,
        IsSuccess = false
    };
}

/// <inheritdoc />
public class Response : IResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Response"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor is protected to prevent the creation of instances of this class.
    /// </remarks>
    private Response()
    {
    }

    /// <inheritdoc />
    public required bool IsSuccess { get; init; }

    /// <inheritdoc />
    public string? ErrorMessage { get; init; }

    /// <inheritdoc />
    public int? ErrorCode { get; init; }

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <returns>A successful response.</returns>
    public static Response Success() => new()
    {
        IsSuccess = true
    };

    /// <summary>
    /// Creates a failed response.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    /// <returns>A failed response with the specified error message and error code.</returns>
    public static Response Fail(string errorMessage, int errorCode) => new()
    {
        ErrorMessage = errorMessage,
        ErrorCode = errorCode,
        IsSuccess = false
    };
}
