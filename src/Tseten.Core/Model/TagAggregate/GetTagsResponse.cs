// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Validation;

namespace Tseten.Core;

public class GetTagsResponse : ResponseBase
{
    public List<TagDto> Tags { get; set; } = [];
}

