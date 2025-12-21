// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Tseten.Core.Model.SoftwareRequirement;

namespace Tseten.SoftwareRequirements.Api.RequestHandlers;

public class UpdateSoftwareRequirementHandler: IRequestHandler<UpdateSoftwareRequirementRequest, UpdateSoftwareRequirementResponse>
{
    private readonly ILogger<UpdateSoftwareRequirementHandler> _logger;

    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public UpdateSoftwareRequirementHandler(ILogger<UpdateSoftwareRequirementHandler> logger,ISoftwareRequirementsRepository softwareRequirementsRepository){
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(softwareRequirementsRepository);

        _logger = logger;
        _softwareRequirementsRepository = softwareRequirementsRepository;

    }

    public async Task<UpdateSoftwareRequirementResponse> Handle(UpdateSoftwareRequirementRequest request,CancellationToken cancellationToken)
    {
        var softwareRequirement = _softwareRequirementsRepository.GetById(request.SoftwareRequirementId);

        softwareRequirement.ParentSoftwareRequirementId = request.ParentSoftwareRequirementId;
        softwareRequirement.Description = request.Description;
        softwareRequirement.CanImplement = request.CanImplement;
        softwareRequirement.CanTest = request.CanTest;
        softwareRequirement.AcceptanceCriteria = request.AcceptanceCriteria ?? [];

        return new()
        {
            SoftwareRequirement = softwareRequirement.ToDto()
        };

    }

}

