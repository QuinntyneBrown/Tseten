// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Api;

public static class TagExtensions
{
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Description = tag.Description
        };
    }
}
