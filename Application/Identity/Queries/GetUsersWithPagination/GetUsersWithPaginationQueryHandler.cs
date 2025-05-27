using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Domain.Identity;
using Shared.Common.Response;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Queries.GetUsersWithPagination;

/// <summary>
/// Handler for GetUsersWithPaginationQuery
/// </summary>
public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, PagedResponse<UserDetailsDto, object>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Constructor
    /// </summary>
    public GetUsersWithPaginationQueryHandler(
        UserManager<ApplicationUser> userManager,
        IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    /// <summary>
    /// Handle the query
    /// </summary>
    public async Task<PagedResponse<UserDetailsDto, object>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Start with all users
            var query = _userManager.Users.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                var searchTerm = request.SearchString.ToLower();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    (u.FirstName != null && u.FirstName.ToLower().Contains(searchTerm)) ||
                    (u.LastName != null && u.LastName.ToLower().Contains(searchTerm)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm))
                );
            }

            // Filter by active status
            if (!request.IncludeInactive)
            {
                query = query.Where(u => u.IsActive);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var pagedUsers = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var userDtos = new List<UserDetailsDto>();
            foreach (var user in pagedUsers)
            {
                var userDto = user.Adapt<UserDetailsDto>();

                // Get roles for the user
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();

                userDtos.Add(userDto);
            }

            // Filter by roles if specified
            if (request.Roles != null && request.Roles.Any())
            {
                userDtos = userDtos
                    .Where(u => u.Roles.Any(r =>
                        request.Roles.Contains(r, StringComparer.OrdinalIgnoreCase)))
                    .ToList();

                // Recalculate total count if filtering by roles
                totalCount = userDtos.Count;
            }

            return PagedResponse<UserDetailsDto, object>.Success(userDtos, totalCount);
        }
        catch (Exception ex)
        {
            return PagedResponse<UserDetailsDto, object>.Failure($"Error retrieving users: {ex.Message}");
        }
    }
}
