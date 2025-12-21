// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.Tag;

public class GetTagsHandler : IRequestHandler<GetTagsRequest, GetTagsResponse>
{
    private readonly ITsetenContext _context;
    private readonly ILogger<GetTagsHandler> _logger;

    public GetTagsHandler(ITsetenContext context, ILogger<GetTagsHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<GetTagsResponse> Handle(GetTagsRequest request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} tags", tags.Count);

        return new GetTagsResponse
        {
            Tags = tags.Select(t => t.ToDto()).ToList()
        };
    }
}
