// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class CreateSoftwareRequirementHandler : IRequestHandler<CreateSoftwareRequirementRequest, CreateSoftwareRequirementResponse>
{
    private readonly ILogger<CreateSoftwareRequirementHandler> _logger;
    private readonly ITsetenContext _context;

    public CreateSoftwareRequirementHandler(ILogger<CreateSoftwareRequirementHandler> logger, ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _context = context;
    }

    public async Task<CreateSoftwareRequirementResponse> Handle(CreateSoftwareRequirementRequest request, CancellationToken cancellationToken)
    {
        var softwareRequirement = new SoftwareRequirement
        {
            SoftwareRequirementId = request.SoftwareRequirementId,
            ParentSoftwareRequirementId = request.ParentSoftwareRequirementId,
            Description = request.Description,
            CanImplement = request.CanImplement,
            CanTest = request.CanTest,
            Comments = request.Comments ?? [],
            AcceptanceCriteria = request.AcceptanceCriteria ?? []
        };

        _context.SoftwareRequirements.Add(softwareRequirement);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created software requirement {SoftwareRequirementId}", softwareRequirement.SoftwareRequirementId);

        return new CreateSoftwareRequirementResponse
        {
            SoftwareRequirement = softwareRequirement.ToDto()
        };
    }
}
