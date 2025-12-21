// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Validation;

namespace Tseten.Models.SoftwareRequirement;

public class UpdateSoftwareRequirementResponse : ResponseBase
{
    public SoftwareRequirementDto? SoftwareRequirement { get; set; }
}

