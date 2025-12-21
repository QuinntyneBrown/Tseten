// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class DeleteSoftwareRequirementHandler: IRequestHandler<DeleteSoftwareRequirementRequest, DeleteSoftwareRequirementResponse>
{
    private readonly ILogger<DeleteSoftwareRequirementHandler> _logger;

    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public DeleteSoftwareRequirementHandler(ILogger<DeleteSoftwareRequirementHandler> logger,ISoftwareRequirementsRepository softwareRequirementsRepository){
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(softwareRequirementsRepository);

        _logger = logger;
        _softwareRequirementsRepository = softwareRequirementsRepository;

    }

    public async Task<DeleteSoftwareRequirementResponse> Handle(DeleteSoftwareRequirementRequest request,CancellationToken cancellationToken)
    {        
        _softwareRequirementsRepository.Delete(request.SoftwareRequirementId);

        return new();
    }
}

