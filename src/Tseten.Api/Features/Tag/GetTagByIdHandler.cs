// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.Tag;

public class GetTagByIdHandler : IRequestHandler<GetTagByIdRequest, GetTagByIdResponse>
{
    private readonly ITsetenContext _context;
    private readonly ILogger<GetTagByIdHandler> _logger;

    public GetTagByIdHandler(ITsetenContext context, ILogger<GetTagByIdHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<GetTagByIdResponse> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TagId == request.TagId, cancellationToken);

        if (tag == null)
        {
            _logger.LogWarning("Tag {TagId} not found", request.TagId);
            return new GetTagByIdResponse();
        }

        _logger.LogInformation("Retrieved tag {TagId}", tag.TagId);

        return new GetTagByIdResponse
        {
            Tag = tag.ToDto()
        };
    }
}
