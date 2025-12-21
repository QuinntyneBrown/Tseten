// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly ITsetenContext _context;

    public GetUserByIdHandler(ITsetenContext context)
    {
        _context = context;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Privileges)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        return new GetUserByIdResponse
        {
            User = user?.ToDto()
        };
    }
}
