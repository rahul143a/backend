using Abstraction.Identity.Dtos;
using Shared.Common.Response;
using MediatR;

namespace Application.Identity.Queries.GetUsersWithPagination;

/// <summary>
/// Query to get users with pagination
/// </summary>
public class GetUsersWithPaginationQuery : IRequest<PagedResponse<UserDetailsDto, object>>
{
    /// <summary>
    /// Page number (1-based)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Search string
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Whether to include inactive users
    /// </summary>
    public bool IncludeInactive { get; set; } = false;

    /// <summary>
    /// Roles to filter by
    /// </summary>
    public List<string>? Roles { get; set; }
}
