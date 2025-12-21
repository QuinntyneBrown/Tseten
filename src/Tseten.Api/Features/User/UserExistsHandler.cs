// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class UserExistsHandler : IRequestHandler<UserExistsRequest, UserExistsResponse>
{
    private readonly ITsetenContext _context;

    public UserExistsHandler(ITsetenContext context)
    {
        _context = context;
    }

    public async Task<UserExistsResponse> Handle(UserExistsRequest request, CancellationToken cancellationToken)
    {
        var exists = await _context.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Username.ToLower() == request.Username.ToLower(), cancellationToken);

        return new UserExistsResponse { Exists = exists };
    }
}
