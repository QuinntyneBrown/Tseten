// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using SpecSync.Models.SoftwareRequirement;

namespace SpecSync.SoftwareRequirements.Api.RequestHandlers;

public class CreateSoftwareRequirementHandler: IRequestHandler<CreateSoftwareRequirementRequest, CreateSoftwareRequirementResponse>
{
    private readonly ILogger<CreateSoftwareRequirementHandler> _logger;

    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public CreateSoftwareRequirementHandler(ILogger<CreateSoftwareRequirementHandler> logger,ISoftwareRequirementsRepository softwareRequirementsRepository){
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(softwareRequirementsRepository);

        _logger = logger;
        _softwareRequirementsRepository = softwareRequirementsRepository;

    }

    public async Task<CreateSoftwareRequirementResponse> Handle(CreateSoftwareRequirementRequest request,CancellationToken cancellationToken)
    {
        var softwareRequirement = new SoftwareRequirement()
        {
            SoftwareRequirementId = request.SoftwareRequirementId,
            ParentSoftwareRequirementId = request.ParentSoftwareRequirementId,
            Description = request.Description,
            CanImplement = request.CanImplement,
            CanTest = request.CanTest,
            Comments = request.Comments,
            AcceptanceCriteria = request.AcceptanceCriteria ?? []
        };

        _softwareRequirementsRepository.Create(softwareRequirement);

        return new() { 

            SoftwareRequirement = softwareRequirement.ToDto(),
        };

    }

}

