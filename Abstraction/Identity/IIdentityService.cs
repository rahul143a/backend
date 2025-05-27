using Abstraction.Identity.Dtos;
using Shared.Common;

namespace Abstraction.Identity;

/// <summary>
/// Interface for identity service
/// </summary>
public interface IIdentityService : ITransientService
{
    /// <summary>
    /// Get a user by ID
    /// </summary>
    Task<UserDetailsDto?> GetUserAsync(string userId);

    /// <summary>
    /// Get a list of users
    /// </summary>
    Task<List<UserDetailsDto>> GetUsersAsync();

    /// <summary>
    /// Get a list of users by role
    /// </summary>
    Task<List<UserDetailsDto>> GetUsersInRoleAsync(string roleName);

    /// <summary>
    /// Check if a user is in a role
    /// </summary>
    Task<bool> IsInRoleAsync(string userId, string role);

    /// <summary>
    /// Get a user's roles
    /// </summary>
    Task<List<string>> GetUserRolesAsync(string userId);

    /// <summary>
    /// Register a new user
    /// </summary>
    Task<string> RegisterUserAsync(RegisterUserRequest request, string origin);

    /// <summary>
    /// Confirm email
    /// </summary>
    Task<string> ConfirmEmailAsync(string userId, string code);

    /// <summary>
    /// Confirm phone number
    /// </summary>
    Task<string> ConfirmPhoneNumberAsync(string userId, string code);

    /// <summary>
    /// Forgot password
    /// </summary>
    Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

    /// <summary>
    /// Reset password
    /// </summary>
    Task<string> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// Update a user
    /// </summary>
    Task<string> UpdateUserAsync(UpdateUserRequest request, string userId);

    /// <summary>
    /// Change password
    /// </summary>
    Task<string> ChangePasswordAsync(ChangePasswordRequest request, string userId);
}
