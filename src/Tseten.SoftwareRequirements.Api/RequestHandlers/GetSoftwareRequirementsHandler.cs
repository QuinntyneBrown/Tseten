// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Tseten.Core.Model.SoftwareRequirement;

namespace Tseten.SoftwareRequirements.Api.RequestHandlers;

public class GetSoftwareRequirementsHandler: IRequestHandler<GetSoftwareRequirementsRequest, GetSoftwareRequirementsResponse>
{
    private readonly ILogger<GetSoftwareRequirementsHandler> _logger;

    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public GetSoftwareRequirementsHandler(ILogger<GetSoftwareRequirementsHandler> logger,ISoftwareRequirementsRepository softwareRequirementsRepository){
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(softwareRequirementsRepository);

        _logger = logger;
        _softwareRequirementsRepository = softwareRequirementsRepository;

    }

    public async Task<GetSoftwareRequirementsResponse> Handle(GetSoftwareRequirementsRequest request,CancellationToken cancellationToken)
    {
        var softwareRequirements = _softwareRequirementsRepository.Get();

        return new GetSoftwareRequirementsResponse()
        {
            SoftwareRequirements = softwareRequirements
            .Select(x => x.ToDto())
            .ToList()
        };

    }

}

