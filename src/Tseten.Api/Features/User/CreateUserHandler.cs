// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Api.Features.User;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly ITsetenContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(ITsetenContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var salt = _passwordHasher.GenerateSalt();

        var roles = await _context.Roles
            .Where(r => request.Roles.Contains(r.Name))
            .ToListAsync(cancellationToken);

        var user = new Core.User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Password = _passwordHasher.HashPassword(request.Password, salt),
            Salt = salt,
            Roles = roles
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse
        {
            User = user.ToDto()
        };
    }
}
