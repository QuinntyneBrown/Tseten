// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.Tag;

public class UpdateTagHandler : IRequestHandler<UpdateTagRequest, UpdateTagResponse>
{
    private readonly ITsetenContext _context;
    private readonly ILogger<UpdateTagHandler> _logger;

    public UpdateTagHandler(ITsetenContext context, ILogger<UpdateTagHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<UpdateTagResponse> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.TagId == request.TagId, cancellationToken);

        if (tag == null)
        {
            _logger.LogWarning("Tag {TagId} not found for update", request.TagId);
            return new UpdateTagResponse
            {
                Errors = ["Tag not found"]
            };
        }

        tag.Name = request.Name;
        tag.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated tag {TagId}", tag.TagId);

        return new UpdateTagResponse
        {
            Tag = tag.ToDto()
        };
    }
}
