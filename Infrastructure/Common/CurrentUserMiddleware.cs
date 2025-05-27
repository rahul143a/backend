using Abstraction.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common;

/// <summary>
/// Middleware for initializing the current user
/// </summary>
public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Process the request
    /// </summary>
    public async Task InvokeAsync(HttpContext context, ICurrentUserInitializer currentUserInitializer)
    {
        currentUserInitializer.SetCurrentUser(context.User);
        await _next(context);
    }
}
