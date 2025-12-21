// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
{
    private readonly ITsetenContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticateHandler> _logger;

    public AuthenticateHandler(
        ITsetenContext context,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<AuthenticateHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Privileges)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Authentication failed: user not found for username {Username}", request.Username);
            return new AuthenticateResponse
            {
                Errors = ["Invalid username or password"]
            };
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.Password, user.Salt))
        {
            _logger.LogWarning("Authentication failed: invalid password for username {Username}", request.Username);
            return new AuthenticateResponse
            {
                Errors = ["Invalid username or password"]
            };
        }

        var token = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _logger.LogInformation("User {Username} authenticated successfully", request.Username);

        return new AuthenticateResponse
        {
            UserId = user.UserId,
            Token = token,
            RefreshToken = refreshToken
        };
    }
}
