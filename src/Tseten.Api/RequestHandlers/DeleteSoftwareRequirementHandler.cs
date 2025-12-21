// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class DeleteSoftwareRequirementHandler : IRequestHandler<DeleteSoftwareRequirementRequest, DeleteSoftwareRequirementResponse>
{
    private readonly ILogger<DeleteSoftwareRequirementHandler> _logger;
    private readonly ITsetenContext _context;

    public DeleteSoftwareRequirementHandler(ILogger<DeleteSoftwareRequirementHandler> logger, ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _context = context;
    }

    public async Task<DeleteSoftwareRequirementResponse> Handle(DeleteSoftwareRequirementRequest request, CancellationToken cancellationToken)
    {
        var softwareRequirement = await _context.SoftwareRequirements
            .FirstOrDefaultAsync(sr => sr.SoftwareRequirementId == request.SoftwareRequirementId, cancellationToken);

        if (softwareRequirement == null)
        {
            _logger.LogWarning("Software requirement {SoftwareRequirementId} not found for deletion", request.SoftwareRequirementId);
            return new DeleteSoftwareRequirementResponse();
        }

        _context.SoftwareRequirements.Remove(softwareRequirement);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted software requirement {SoftwareRequirementId}", request.SoftwareRequirementId);

        return new DeleteSoftwareRequirementResponse();
    }
}
