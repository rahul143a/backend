namespace Shared.Common.Response;

/// <summary>
/// Paginated result for API responses
/// </summary>
/// <typeparam name="T">Type of the data</typeparam>
public class PaginatedResult<T> : Result<List<T>>
{
    /// <summary>
    /// Current page
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// First page
    /// </summary>
    public int FirstPage { get; set; }

    /// <summary>
    /// Last page
    /// </summary>
    public int LastPage { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total items
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Whether there is a previous page
    /// </summary>
    public bool HasPreviousPage => Page > FirstPage;

    /// <summary>
    /// Whether there is a next page
    /// </summary>
    public bool HasNextPage => Page < LastPage;

    /// <summary>
    /// Create a paginated result
    /// </summary>
    public PaginatedResult(List<T> data, int page, int pageSize, int totalItems)
    {
        Data = data;
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        FirstPage = 1;
        LastPage = TotalPages;
        Succeeded = true;
    }

    /// <summary>
    /// Create a failure result with message
    /// </summary>
    public static PaginatedResult<T> Failure(string message)
    {
        return new PaginatedResult<T>(new List<T>(), 0, 0, 0) { Succeeded = false, Message = message };
    }
}
