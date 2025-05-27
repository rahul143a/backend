using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Host.Controllers;

/// <summary>
/// Base API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    /// <summary>
    /// Mediator instance
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
