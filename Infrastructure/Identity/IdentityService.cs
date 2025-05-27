using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

/// <summary>
/// Implementation of identity service
/// </summary>
public class IdentityService : IIdentityService
{
    /// <summary>
    /// User manager
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Constructor
    /// </summary>
    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    public async Task<UserDetailsDto?> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.Adapt<UserDetailsDto>();
    }

    /// <summary>
    /// Get a list of users
    /// </summary>
    public async Task<List<UserDetailsDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Adapt<List<UserDetailsDto>>();
    }

    /// <summary>
    /// Get a list of users by role
    /// </summary>
    public async Task<List<UserDetailsDto>> GetUsersInRoleAsync(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        return users.Adapt<List<UserDetailsDto>>();
    }

    /// <summary>
    /// Check if a user is in a role
    /// </summary>
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    /// <summary>
    /// Get a user's roles
    /// </summary>
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<string>();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    public async Task<string> RegisterUserAsync(RegisterUserRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            throw new Exception($"User with email {request.Email} already exists.");
        }

        user = await _userManager.FindByNameAsync(request.UserName);
        if (user != null)
        {
            throw new Exception($"User with username {request.UserName} already exists.");
        }

        user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true,
            CreatedOn = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"User {request.UserName} registered.";
    }

    /// <summary>
    /// Confirm email
    /// </summary>
    public async Task<string> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to confirm email: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Email confirmed for user {user.UserName}.";
    }

    /// <summary>
    /// Confirm phone number
    /// </summary>
    public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to confirm phone number: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Phone number confirmed for user {user.UserName}.";
    }

    /// <summary>
    /// Forgot password
    /// </summary>
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception($"User with email {request.Email} not found.");
        }

        // Generate password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // TODO: Send email with reset link

        return $"Password reset token generated for user {user.UserName}.";
    }

    /// <summary>
    /// Reset password
    /// </summary>
    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception($"User with email {request.Email} not found.");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to reset password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Password reset for user {user.UserName}.";
    }

    /// <summary>
    /// Update a user
    /// </summary>
    public async Task<string> UpdateUserAsync(UpdateUserRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.Email;
        user.ImageUrl = request.ImageUrl;
        user.LastModifiedOn = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"User {user.UserName} updated.";
    }

    /// <summary>
    /// Change password
    /// </summary>
    public async Task<string> ChangePasswordAsync(ChangePasswordRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to change password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Password changed for user {user.UserName}.";
    }
}
