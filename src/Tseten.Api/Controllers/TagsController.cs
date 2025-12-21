// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tseten.Core;

namespace Tseten.Api.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : Controller
{
    private readonly ILogger<TagsController> _logger;
    private readonly IMediator _mediator;

    public TagsController(ILogger<TagsController> logger, IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mediator);

        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateTag")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTagRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Errors.Count > 0)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet(Name = "GetTags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var response = await _mediator.Send(new GetTagsRequest());

        return Ok(response);
    }

    [HttpGet("{tagId:guid}", Name = "GetTagById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid tagId)
    {
        var response = await _mediator.Send(new GetTagByIdRequest { TagId = tagId });

        if (response.Tag == null)
        {
            return NotFound(tagId);
        }

        return Ok(response);
    }

    [HttpPut(Name = "UpdateTag")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateTagRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Errors.Count > 0)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{tagId:guid}", Name = "DeleteTag")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid tagId)
    {
        var response = await _mediator.Send(new DeleteTagRequest { TagId = tagId });

        if (response.Errors.Count > 0)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
