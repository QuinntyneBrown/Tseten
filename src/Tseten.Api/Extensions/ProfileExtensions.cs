// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Api;

public static class ProfileExtensions
{
    public static ProfileDto ToDto(this Profile profile)
    {
        return new ProfileDto
        {
            ProfileId = profile.ProfileId,
            UserId = profile.UserId,
            Firstname = profile.Firstname,
            Lastname = profile.Lastname,
            AvatarDigitalAssetId = profile.AvatarDigitalAssetId,
            PhoneNumber = profile.PhoneNumber
        };
    }
}
