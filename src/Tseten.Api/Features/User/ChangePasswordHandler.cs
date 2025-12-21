// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
{
    private readonly ITsetenContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangePasswordHandler(
        ITsetenContext context,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Errors = ["User not authenticated"]
            };
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Errors = ["User not found"]
            };
        }

        if (!_passwordHasher.VerifyPassword(request.OldPassword, user.Password, user.Salt))
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Errors = ["Current password is incorrect"]
            };
        }

        var newSalt = _passwordHasher.GenerateSalt();
        user.Password = _passwordHasher.HashPassword(request.NewPassword, newSalt);
        user.Salt = newSalt;

        await _context.SaveChangesAsync(cancellationToken);

        return new ChangePasswordResponse { Success = true };
    }
}
