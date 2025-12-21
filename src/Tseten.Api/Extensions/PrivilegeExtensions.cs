// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Api;

public static class PrivilegeExtensions
{
    public static PrivilegeDto ToDto(this Privilege privilege)
    {
        return new PrivilegeDto
        {
            PrivilegeId = privilege.PrivilegeId,
            RoleId = privilege.RoleId,
            Aggregate = privilege.Aggregate,
            AccessRight = privilege.AccessRight
        };
    }
}
