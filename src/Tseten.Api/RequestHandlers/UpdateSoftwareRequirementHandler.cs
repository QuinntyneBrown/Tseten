// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class UpdateSoftwareRequirementHandler : IRequestHandler<UpdateSoftwareRequirementRequest, UpdateSoftwareRequirementResponse>
{
    private readonly ILogger<UpdateSoftwareRequirementHandler> _logger;
    private readonly ITsetenContext _context;

    public UpdateSoftwareRequirementHandler(ILogger<UpdateSoftwareRequirementHandler> logger, ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _context = context;
    }

    public async Task<UpdateSoftwareRequirementResponse> Handle(UpdateSoftwareRequirementRequest request, CancellationToken cancellationToken)
    {
        var softwareRequirement = await _context.SoftwareRequirements
            .Include(sr => sr.AcceptanceCriteria)
            .FirstOrDefaultAsync(sr => sr.SoftwareRequirementId == request.SoftwareRequirementId, cancellationToken);

        if (softwareRequirement == null)
        {
            _logger.LogWarning("Software requirement {SoftwareRequirementId} not found for update", request.SoftwareRequirementId);
            return new UpdateSoftwareRequirementResponse
            {
                Errors = ["Software requirement not found"]
            };
        }

        softwareRequirement.ParentSoftwareRequirementId = request.ParentSoftwareRequirementId;
        softwareRequirement.Description = request.Description;
        softwareRequirement.CanImplement = request.CanImplement;
        softwareRequirement.CanTest = request.CanTest;
        softwareRequirement.AcceptanceCriteria = request.AcceptanceCriteria ?? [];

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated software requirement {SoftwareRequirementId}", softwareRequirement.SoftwareRequirementId);

        return new UpdateSoftwareRequirementResponse
        {
            SoftwareRequirement = softwareRequirement.ToDto()
        };
    }
}
