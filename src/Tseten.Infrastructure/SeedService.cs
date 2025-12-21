// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Infrastructure;

public class SeedService
{
    private readonly TsetenContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public SeedService(TsetenContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (!await _context.Roles.AnyAsync())
        {
            var adminRoleId = Guid.NewGuid();
            var memberRoleId = Guid.NewGuid();

            var adminRole = new Role
            {
                RoleId = adminRoleId,
                Name = "SystemAdministrator",
                Privileges =
                [
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "User", AccessRight = AccessRight.Create },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "User", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "User", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "User", AccessRight = AccessRight.Delete },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Role", AccessRight = AccessRight.Create },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Role", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Role", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Role", AccessRight = AccessRight.Delete },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Profile", AccessRight = AccessRight.Create },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Profile", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Profile", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "Profile", AccessRight = AccessRight.Delete },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "InvitationToken", AccessRight = AccessRight.Create },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "InvitationToken", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "InvitationToken", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "InvitationToken", AccessRight = AccessRight.Delete },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "SoftwareRequirement", AccessRight = AccessRight.Create },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "SoftwareRequirement", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "SoftwareRequirement", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = adminRoleId, Aggregate = "SoftwareRequirement", AccessRight = AccessRight.Delete }
                ]
            };

            var memberRole = new Role
            {
                RoleId = memberRoleId,
                Name = "Member",
                Privileges =
                [
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = memberRoleId, Aggregate = "Profile", AccessRight = AccessRight.Read },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = memberRoleId, Aggregate = "Profile", AccessRight = AccessRight.Write },
                    new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = memberRoleId, Aggregate = "SoftwareRequirement", AccessRight = AccessRight.Read }
                ]
            };

            await _context.Roles.AddRangeAsync(adminRole, memberRole);

            var salt = _passwordHasher.GenerateSalt();
            var adminUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = "admin@tseten.com",
                Password = _passwordHasher.HashPassword("Admin123!", salt),
                Salt = salt,
                Roles = [adminRole]
            };

            await _context.Users.AddAsync(adminUser);
            await _context.SaveChangesAsync();
        }
    }
}
