// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Tseten.Core.Model.SoftwareRequirement;

namespace Tseten.SoftwareRequirements.Api.Tests;

public class SoftwareRequirementTests
{
    [Fact]
    public void SoftwareRequirement_ShouldHaveDefaultEmptyAcceptanceCriteriaList()
    {
        // Arrange & Act
        var softwareRequirement = new SoftwareRequirement();

        // Assert
        softwareRequirement.AcceptanceCriteria.Should().NotBeNull();
        softwareRequirement.AcceptanceCriteria.Should().BeEmpty();
    }

    [Fact]
    public void SoftwareRequirement_ShouldContainAcceptanceCriteria()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a user is logged in",
            When = "When the user clicks save",
            Then = "Then data is persisted",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.High
        };

        var softwareRequirement = new SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Test requirement",
            CanImplement = true,
            CanTest = true
        };

        // Act
        softwareRequirement.AcceptanceCriteria.Add(acceptanceCriteria);

        // Assert
        softwareRequirement.AcceptanceCriteria.Should().HaveCount(1);
        softwareRequirement.AcceptanceCriteria[0].Given.Should().Be("Given a user is logged in");
    }

    [Fact]
    public void SoftwareRequirement_ToDto_ShouldMapAcceptanceCriteria()
    {
        // Arrange
        var acceptanceCriteria1 = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given 1",
            When = "When 1",
            Then = "Then 1",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.Low
        };

        var acceptanceCriteria2 = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given 2",
            When = "When 2",
            Then = "Then 2",
            Status = AcceptanceCriteriaStatus.Passed,
            Priority = AcceptanceCriteriaPriority.High
        };

        var softwareRequirement = new SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Test requirement with acceptance criteria",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria> { acceptanceCriteria1, acceptanceCriteria2 }
        };

        // Act
        var dto = softwareRequirement.ToDto();

        // Assert
        dto.AcceptanceCriteria.Should().NotBeNull();
        dto.AcceptanceCriteria.Should().HaveCount(2);
        dto.AcceptanceCriteria![0].Given.Should().Be("Given 1");
        dto.AcceptanceCriteria[1].Given.Should().Be("Given 2");
    }

    [Fact]
    public void SoftwareRequirement_ToDto_ShouldHandleEmptyAcceptanceCriteria()
    {
        // Arrange
        var softwareRequirement = new SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Test requirement without acceptance criteria",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria>()
        };

        // Act
        var dto = softwareRequirement.ToDto();

        // Assert
        dto.AcceptanceCriteria.Should().NotBeNull();
        dto.AcceptanceCriteria.Should().BeEmpty();
    }

    [Fact]
    public void SoftwareRequirement_CanHaveMultipleAcceptanceCriteria()
    {
        // Arrange
        var softwareRequirement = new SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Requirement with many acceptance criteria"
        };

        // Act
        for (int i = 0; i < 10; i++)
        {
            softwareRequirement.AcceptanceCriteria.Add(new AcceptanceCriteria
            {
                AcceptanceCriteriaId = Guid.NewGuid(),
                Given = $"Given {i}",
                When = $"When {i}",
                Then = $"Then {i}",
                Status = AcceptanceCriteriaStatus.Pending,
                Priority = AcceptanceCriteriaPriority.Medium
            });
        }

        // Assert
        softwareRequirement.AcceptanceCriteria.Should().HaveCount(10);
    }
}
