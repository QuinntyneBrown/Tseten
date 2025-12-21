// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class GetSoftwareRequirementByIdHandler : IRequestHandler<GetSoftwareRequirementByIdRequest, GetSoftwareRequirementByIdResponse>
{
    private readonly ILogger<GetSoftwareRequirementByIdHandler> _logger;
    private readonly ITsetenContext _context;

    public GetSoftwareRequirementByIdHandler(ILogger<GetSoftwareRequirementByIdHandler> logger, ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _context = context;
    }

    public async Task<GetSoftwareRequirementByIdResponse> Handle(GetSoftwareRequirementByIdRequest request, CancellationToken cancellationToken)
    {
        var softwareRequirement = await _context.SoftwareRequirements
            .Include(sr => sr.Comments)
            .Include(sr => sr.AcceptanceCriteria)
            .AsNoTracking()
            .FirstOrDefaultAsync(sr => sr.SoftwareRequirementId == request.SoftwareRequirementId, cancellationToken);

        if (softwareRequirement == null)
        {
            _logger.LogWarning("Software requirement {SoftwareRequirementId} not found", request.SoftwareRequirementId);
            return new GetSoftwareRequirementByIdResponse();
        }

        _logger.LogInformation("Retrieved software requirement {SoftwareRequirementId}", request.SoftwareRequirementId);

        return new GetSoftwareRequirementByIdResponse
        {
            SoftwareRequirement = softwareRequirement.ToDto()
        };
    }
}
