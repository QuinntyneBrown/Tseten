// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Models.SoftwareRequirement;

namespace Tseten.Core.Tests.SoftwareRequirement;

public class AcceptanceCriteriaTests
{
    [Fact]
    public void AcceptanceCriteria_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var acceptanceCriteria = new AcceptanceCriteria();

        // Assert
        acceptanceCriteria.AcceptanceCriteriaId.Should().Be(Guid.Empty);
        acceptanceCriteria.Status.Should().Be(AcceptanceCriteriaStatus.Pending);
        acceptanceCriteria.Priority.Should().Be(AcceptanceCriteriaPriority.Low);
    }

    [Fact]
    public void AcceptanceCriteria_ShouldAllowSettingAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddMinutes(10);

        // Act
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = id,
            Given = "Given a user is logged in",
            When = "When the user clicks save",
            Then = "Then data is persisted",
            Status = AcceptanceCriteriaStatus.Passed,
            Priority = AcceptanceCriteriaPriority.High,
            Notes = "Additional notes here",
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Assert
        acceptanceCriteria.AcceptanceCriteriaId.Should().Be(id);
        acceptanceCriteria.Given.Should().Be("Given a user is logged in");
        acceptanceCriteria.When.Should().Be("When the user clicks save");
        acceptanceCriteria.Then.Should().Be("Then data is persisted");
        acceptanceCriteria.Status.Should().Be(AcceptanceCriteriaStatus.Passed);
        acceptanceCriteria.Priority.Should().Be(AcceptanceCriteriaPriority.High);
        acceptanceCriteria.Notes.Should().Be("Additional notes here");
        acceptanceCriteria.CreatedAt.Should().Be(createdAt);
        acceptanceCriteria.UpdatedAt.Should().Be(updatedAt);
    }

    [Theory]
    [InlineData(AcceptanceCriteriaStatus.Pending, 0)]
    [InlineData(AcceptanceCriteriaStatus.InProgress, 1)]
    [InlineData(AcceptanceCriteriaStatus.Passed, 2)]
    [InlineData(AcceptanceCriteriaStatus.Failed, 3)]
    public void AcceptanceCriteriaStatus_ShouldHaveCorrectValues(AcceptanceCriteriaStatus status, int expectedValue)
    {
        // Assert
        ((int)status).Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(AcceptanceCriteriaPriority.Low, 0)]
    [InlineData(AcceptanceCriteriaPriority.Medium, 1)]
    [InlineData(AcceptanceCriteriaPriority.High, 2)]
    public void AcceptanceCriteriaPriority_ShouldHaveCorrectValues(AcceptanceCriteriaPriority priority, int expectedValue)
    {
        // Assert
        ((int)priority).Should().Be(expectedValue);
    }

    [Fact]
    public void AcceptanceCriteria_UpdatedAt_ShouldBeNullable()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "Given",
            When = "When",
            Then = "Then"
        };

        // Assert
        acceptanceCriteria.UpdatedAt.Should().BeNull();
    }
}
