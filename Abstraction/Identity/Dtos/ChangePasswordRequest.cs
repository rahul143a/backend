namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for changing password
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Current password
    /// </summary>
    public string CurrentPassword { get; set; } = default!;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = default!;

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string ConfirmNewPassword { get; set; } = default!;
}
