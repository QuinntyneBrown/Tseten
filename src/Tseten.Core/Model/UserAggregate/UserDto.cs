// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid? DefaultProfileId { get; set; }
    public List<RoleDto> Roles { get; set; } = [];
}
