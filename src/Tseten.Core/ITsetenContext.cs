// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Core;

namespace Tseten.Core;

public interface ITsetenContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Privilege> Privileges { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<InvitationToken> InvitationTokens { get; }
    DbSet<SoftwareRequirementEmbedding> SoftwareRequirementEmbeddings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
