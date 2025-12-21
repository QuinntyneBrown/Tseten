// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Tseten.Core;

public class ChangePasswordRequest : IRequest<ChangePasswordResponse>
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
}
