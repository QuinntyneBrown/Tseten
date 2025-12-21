// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Controllers;

[ApiController]
[Route("api/softwarerequirements")]
public class SoftwareRequirementsController: Controller
{
    private readonly ILogger<SoftwareRequirementsController> _logger;

    private readonly IMediator _mediator;

    public SoftwareRequirementsController(ILogger<SoftwareRequirementsController> logger,IMediator mediator){
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mediator);

        _logger = logger;
        _mediator = mediator;

    }

    [HttpPost(Name = "Create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody]CreateSoftwareRequirementRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);

    }

    [HttpPut(Name = "Update")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody]UpdateSoftwareRequirementRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);

    }

    [HttpDelete("{softwareRequirementId}", Name = "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute]DeleteSoftwareRequirementRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);

    }

    [HttpGet(Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromRoute]GetSoftwareRequirementsRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);

    }

    [HttpGet("{softwareRequirementId}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute]GetSoftwareRequirementByIdRequest request)
    {
        var response = await _mediator.Send(request);

        if(response.SoftwareRequirement == null)
        {
            return NotFound(request.SoftwareRequirementId);
        }

        return Ok(response);

    }

    /// <summary>
    /// Searches software requirements using semantic vector similarity.
    /// Supports pagination and optional filtering.
    /// </summary>
    /// <param name="request">The search request with query and pagination parameters.</param>
    /// <returns>Paginated search results with similarity scores.</returns>
    [HttpGet("search", Name = "Search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromQuery]SearchSoftwareRequirementsRequest request)
    {
        var response = await _mediator.Send(request);

        if(response.Errors?.Any() == true)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Imports/syncs all software requirements into the vector database for semantic search.
    /// This should be called after adding or updating software requirements.
    /// </summary>
    /// <returns>Import result with count of imported requirements.</returns>
    [HttpPost("embeddings/import", Name = "ImportEmbeddings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportEmbeddingsAsync()
    {
        var response = await _mediator.Send(new ImportEmbeddingsRequest());

        if(!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

}

