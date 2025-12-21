// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Tseten.Core;

public class UserExistsRequest : IRequest<UserExistsResponse>
{
    public string Username { get; set; } = string.Empty;
}
