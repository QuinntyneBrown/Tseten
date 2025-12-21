// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.Tag;

public class DeleteTagHandler : IRequestHandler<DeleteTagRequest, DeleteTagResponse>
{
    private readonly ITsetenContext _context;
    private readonly ILogger<DeleteTagHandler> _logger;

    public DeleteTagHandler(ITsetenContext context, ILogger<DeleteTagHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<DeleteTagResponse> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.TagId == request.TagId, cancellationToken);

        if (tag == null)
        {
            _logger.LogWarning("Tag {TagId} not found for deletion", request.TagId);
            return new DeleteTagResponse
            {
                Errors = ["Tag not found"]
            };
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted tag {TagId}", request.TagId);

        return new DeleteTagResponse();
    }
}
