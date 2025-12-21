// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tseten.Core;

namespace Tseten.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mediator);

        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Errors?.Any() == true)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("current")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var response = await _mediator.Send(new GetCurrentUserRequest());

        if (response.User == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    [HttpGet("exists/{username}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExistsAsync([FromRoute] string username)
    {
        var response = await _mediator.Send(new UserExistsRequest { Username = username });
        return Ok(response.Exists);
    }

    [HttpPost("change-password")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Errors?.Any() == true)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {
        var response = await _mediator.Send(new GetUsersRequest());
        return Ok(response);
    }

    [HttpGet("{userId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid userId)
    {
        var response = await _mediator.Send(new GetUserByIdRequest { UserId = userId });

        if (response.User == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
