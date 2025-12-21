// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Tseten.Core.Model.SoftwareRequirement;

namespace Tseten.SoftwareRequirements.Api.Tests;

public class AcceptanceCriteriaTests
{
    [Fact]
    public void AcceptanceCriteria_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var acceptanceCriteria = new AcceptanceCriteria();

        // Assert
        acceptanceCriteria.AcceptanceCriteriaId.Should().Be(Guid.Empty);
        acceptanceCriteria.Given.Should().BeNull();
        acceptanceCriteria.When.Should().BeNull();
        acceptanceCriteria.Then.Should().BeNull();
        acceptanceCriteria.Status.Should().Be(AcceptanceCriteriaStatus.Pending);
        acceptanceCriteria.Priority.Should().Be(AcceptanceCriteriaPriority.Low);
        acceptanceCriteria.Notes.Should().BeNull();
    }

    [Fact]
    public void AcceptanceCriteria_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var given = "Given a user is logged in";
        var when = "When the user clicks the save button";
        var then = "Then the data should be persisted";
        var status = AcceptanceCriteriaStatus.Passed;
        var priority = AcceptanceCriteriaPriority.High;
        var notes = "Important acceptance criteria";
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddHours(1);

        // Act
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = id,
            Given = given,
            When = when,
            Then = then,
            Status = status,
            Priority = priority,
            Notes = notes,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Assert
        acceptanceCriteria.AcceptanceCriteriaId.Should().Be(id);
        acceptanceCriteria.Given.Should().Be(given);
        acceptanceCriteria.When.Should().Be(when);
        acceptanceCriteria.Then.Should().Be(then);
        acceptanceCriteria.Status.Should().Be(status);
        acceptanceCriteria.Priority.Should().Be(priority);
        acceptanceCriteria.Notes.Should().Be(notes);
        acceptanceCriteria.CreatedAt.Should().Be(createdAt);
        acceptanceCriteria.UpdatedAt.Should().Be(updatedAt);
    }

    [Theory]
    [InlineData(AcceptanceCriteriaStatus.Pending)]
    [InlineData(AcceptanceCriteriaStatus.Passed)]
    [InlineData(AcceptanceCriteriaStatus.Failed)]
    [InlineData(AcceptanceCriteriaStatus.NotApplicable)]
    public void AcceptanceCriteriaStatus_ShouldHaveAllValues(AcceptanceCriteriaStatus status)
    {
        // Arrange & Act
        var acceptanceCriteria = new AcceptanceCriteria { Status = status };

        // Assert
        acceptanceCriteria.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(AcceptanceCriteriaPriority.Low)]
    [InlineData(AcceptanceCriteriaPriority.Medium)]
    [InlineData(AcceptanceCriteriaPriority.High)]
    [InlineData(AcceptanceCriteriaPriority.Critical)]
    public void AcceptanceCriteriaPriority_ShouldHaveAllValues(AcceptanceCriteriaPriority priority)
    {
        // Arrange & Act
        var acceptanceCriteria = new AcceptanceCriteria { Priority = priority };

        // Assert
        acceptanceCriteria.Priority.Should().Be(priority);
    }
}
