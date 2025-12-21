// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Tseten.Core;

namespace Tseten.Api.Features.Tag;

public class CreateTagHandler : IRequestHandler<CreateTagRequest, CreateTagResponse>
{
    private readonly ITsetenContext _context;
    private readonly ILogger<CreateTagHandler> _logger;

    public CreateTagHandler(ITsetenContext context, ILogger<CreateTagHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<CreateTagResponse> Handle(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = new Tseten.Core.Tag
        {
            TagId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created tag {TagId} with name {Name}", tag.TagId, tag.Name);

        return new CreateTagResponse
        {
            Tag = tag.ToDto()
        };
    }
}
