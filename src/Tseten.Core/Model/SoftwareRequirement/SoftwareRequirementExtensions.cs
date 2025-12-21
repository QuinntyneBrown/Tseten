// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core.Model.SoftwareRequirement;

public static class SoftwareRequirementExtensions
{
    public static SoftwareRequirementDto ToDto(this SoftwareRequirement softwareRequirement)
    {
        return new SoftwareRequirementDto()
        {
            SoftwareRequirementId = softwareRequirement.SoftwareRequirementId,
            ParentSoftwareRequirementId = softwareRequirement.ParentSoftwareRequirementId,
            Description = softwareRequirement.Description,
            CanImplement = softwareRequirement.CanImplement,
            CanTest = softwareRequirement.CanTest,
            Comments = softwareRequirement.Comments,
            AcceptanceCriteria = softwareRequirement.AcceptanceCriteria?.ToDto(),
        };

    }

}

