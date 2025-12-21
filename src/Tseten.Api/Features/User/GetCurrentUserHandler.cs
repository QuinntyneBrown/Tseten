// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    private readonly ITsetenContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserHandler(ITsetenContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return new GetCurrentUserResponse { User = null };
        }

        var user = await _context.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Privileges)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            return new GetCurrentUserResponse { User = null };
        }

        return new GetCurrentUserResponse
        {
            User = user.ToDto()
        };
    }
}
