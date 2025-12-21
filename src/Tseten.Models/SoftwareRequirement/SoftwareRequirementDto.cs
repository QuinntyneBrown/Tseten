// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Models.SoftwareRequirement;

public class SoftwareRequirementDto
{
    public string SoftwareRequirementId { get; set; }
    public string ParentSoftwareRequirementId { get; set; }
    public string Description { get; set; }
    public bool CanImplement { get; set; }
    public bool CanTest { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<AcceptanceCriteriaDto>? AcceptanceCriteria { get; set; }
}

