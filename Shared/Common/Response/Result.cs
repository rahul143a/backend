using System.Net;

namespace Shared.Common.Response;

/// <summary>
/// Generic result class for API responses
/// </summary>
/// <typeparam name="T">Type of the data</typeparam>
public class Result<T>
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message (if any)
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// Create a success result with data
    /// </summary>
    public static Result<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data };
    }

    /// <summary>
    /// Create a success result with message
    /// </summary>
    public static Result<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Message = message };
    }

    /// <summary>
    /// Create a success result with data and message
    /// </summary>
    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Message = message };
    }

    /// <summary>
    /// Create a failure result with message
    /// </summary>
    public static Result<T> Failure(string message)
    {
        return new Result<T> { Succeeded = false, Message = message };
    }

    /// <summary>
    /// Create a failure result with message and status code
    /// </summary>
    public static Result<T> Failure(string message, HttpStatusCode statusCode)
    {
        return new Result<T> { Succeeded = false, Message = message, StatusCode = statusCode };
    }
}
