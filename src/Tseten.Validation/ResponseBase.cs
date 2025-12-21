// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Validation;

public class ResponseBase
{
    public ResponseBase()
    {
        Errors = [];
    }

    public List<string> Errors { get; set; }
}
