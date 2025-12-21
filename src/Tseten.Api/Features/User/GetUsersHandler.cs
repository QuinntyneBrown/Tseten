// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    private readonly ITsetenContext _context;

    public GetUsersHandler(ITsetenContext context)
    {
        _context = context;
    }

    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Privileges)
            .ToListAsync(cancellationToken);

        return new GetUsersResponse
        {
            Users = users.Select(u => u.ToDto()).ToList()
        };
    }
}
