// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Core.Model.SoftwareRequirement;

public static class AcceptanceCriteriaExtensions
{
    public static AcceptanceCriteriaDto ToDto(this AcceptanceCriteria acceptanceCriteria)
    {
        return new AcceptanceCriteriaDto
        {
            AcceptanceCriteriaId = acceptanceCriteria.AcceptanceCriteriaId,
            Given = acceptanceCriteria.Given,
            When = acceptanceCriteria.When,
            Then = acceptanceCriteria.Then,
            Status = acceptanceCriteria.Status,
            Priority = acceptanceCriteria.Priority,
            Notes = acceptanceCriteria.Notes,
            CreatedAt = acceptanceCriteria.CreatedAt,
            UpdatedAt = acceptanceCriteria.UpdatedAt
        };
    }

    public static List<AcceptanceCriteriaDto> ToDto(this List<AcceptanceCriteria> acceptanceCriteriaList)
    {
        return acceptanceCriteriaList?.Select(ac => ac.ToDto()).ToList() ?? new List<AcceptanceCriteriaDto>();
    }
}
