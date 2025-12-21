// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public byte[] Salt { get; set; } = [];
    public Guid? CurrentProfileId { get; set; }
    public Guid? DefaultProfileId { get; set; }
    public bool IsDeleted { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<Profile> Profiles { get; set; } = [];
}
