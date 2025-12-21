// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core;

public class CreateProfileResponse
{
    public ProfileDto? Profile { get; set; }
    public List<string>? Errors { get; set; }
}
