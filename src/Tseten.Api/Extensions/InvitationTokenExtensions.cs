// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Api;

public static class InvitationTokenExtensions
{
    public static InvitationTokenDto ToDto(this InvitationToken invitationToken)
    {
        return new InvitationTokenDto
        {
            InvitationTokenId = invitationToken.InvitationTokenId,
            Value = invitationToken.Value,
            Expiry = invitationToken.Expiry,
            Type = invitationToken.Type
        };
    }
}
