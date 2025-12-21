// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.RequestHandlers;

public class GetSoftwareRequirementsHandler : IRequestHandler<GetSoftwareRequirementsRequest, GetSoftwareRequirementsResponse>
{
    private readonly ILogger<GetSoftwareRequirementsHandler> _logger;
    private readonly ITsetenContext _context;

    public GetSoftwareRequirementsHandler(ILogger<GetSoftwareRequirementsHandler> logger, ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _context = context;
    }

    public async Task<GetSoftwareRequirementsResponse> Handle(GetSoftwareRequirementsRequest request, CancellationToken cancellationToken)
    {
        var softwareRequirements = await _context.SoftwareRequirements
            .Include(sr => sr.Comments)
            .Include(sr => sr.AcceptanceCriteria)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} software requirements", softwareRequirements.Count);

        return new GetSoftwareRequirementsResponse
        {
            SoftwareRequirements = softwareRequirements.Select(x => x.ToDto()).ToList()
        };
    }
}
