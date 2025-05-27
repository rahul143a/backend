using Abstraction.Identity.Dtos;
using Shared.Common;

namespace Abstraction.Identity;

/// <summary>
/// Interface for token service
/// </summary>
public interface ITokenService : ITransientService
{
    /// <summary>
    /// Get a token for the specified request
    /// </summary>
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress);

    /// <summary>
    /// Refresh a token
    /// </summary>
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}
