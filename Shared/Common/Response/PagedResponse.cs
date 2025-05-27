using System.Net;

namespace Shared.Common.Response;

/// <summary>
/// Paged response for API responses based on the engageto-be project format
/// </summary>
/// <typeparam name="T">Type of the items</typeparam>
/// <typeparam name="TData">Type of additional data</typeparam>
public class PagedResponse<T, TData>
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Succeeded { get; set; } = true;

    /// <summary>
    /// Response message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Additional data
    /// </summary>
    public TData? Data { get; set; }

    /// <summary>
    /// List of errors
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Items in the current page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Number of items in the current page
    /// </summary>
    public int ItemsCount => Items?.Count() ?? 0;

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// Default constructor
    /// </summary>
    public PagedResponse()
    {
    }

    /// <summary>
    /// Constructor with items and count
    /// </summary>
    public PagedResponse(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
        Data = default;
    }

    /// <summary>
    /// Constructor with items, count, and data
    /// </summary>
    public PagedResponse(IEnumerable<T> items, int totalCount, TData data)
    {
        Items = items;
        TotalCount = totalCount;
        Data = data;
    }

    /// <summary>
    /// Constructor with data only
    /// </summary>
    public PagedResponse(TData data)
    {
        Data = data;
        Items = new List<T>();
    }

    /// <summary>
    /// Create a success response with items and count
    /// </summary>
    public static PagedResponse<T, TData> Success(IEnumerable<T> items, int totalCount)
    {
        return new PagedResponse<T, TData>
        {
            Succeeded = true,
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Create a success response with items, count, and data
    /// </summary>
    public static PagedResponse<T, TData> Success(IEnumerable<T> items, int totalCount, TData data)
    {
        return new PagedResponse<T, TData>
        {
            Succeeded = true,
            Items = items,
            TotalCount = totalCount,
            Data = data
        };
    }

    /// <summary>
    /// Create a failure response with message
    /// </summary>
    public static PagedResponse<T, TData> Failure(string message)
    {
        return new PagedResponse<T, TData>
        {
            Succeeded = false,
            Message = message,
            Items = new List<T>()
        };
    }

    /// <summary>
    /// Create a failure response with errors
    /// </summary>
    public static PagedResponse<T, TData> Failure(List<string> errors)
    {
        return new PagedResponse<T, TData>
        {
            Succeeded = false,
            Errors = errors,
            Items = new List<T>()
        };
    }
}
