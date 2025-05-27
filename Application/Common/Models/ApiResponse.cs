namespace Application.Common.Models;

/// <summary>
/// API response model
/// </summary>
/// <typeparam name="T">Type of the data</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Whether the request was successful
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public ApiResponse(T? data = default, string? message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public ApiResponse(bool succeeded, string? message = null, T? data = default)
    {
        Succeeded = succeeded;
        Message = message;
        Data = data;
    }
}
