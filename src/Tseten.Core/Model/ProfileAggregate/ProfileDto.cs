// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core;

public class ProfileDto
{
    public Guid ProfileId { get; set; }
    public Guid UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public Guid? AvatarDigitalAssetId { get; set; }
    public string? PhoneNumber { get; set; }
}
