// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Models.SoftwareRequirement;

public class AcceptanceCriteriaDto
{
    public Guid AcceptanceCriteriaId { get; set; }
    public string Given { get; set; }
    public string When { get; set; }
    public string Then { get; set; }
    public AcceptanceCriteriaStatus Status { get; set; }
    public AcceptanceCriteriaPriority Priority { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
