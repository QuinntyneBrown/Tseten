// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Tests;

public class AcceptanceCriteriaDtoTests
{
    [Fact]
    public void AcceptanceCriteriaDto_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var dto = new AcceptanceCriteriaDto();

        // Assert
        dto.AcceptanceCriteriaId.Should().Be(Guid.Empty);
        dto.Given.Should().BeNull();
        dto.When.Should().BeNull();
        dto.Then.Should().BeNull();
        dto.Status.Should().Be(AcceptanceCriteriaStatus.Pending);
        dto.Priority.Should().Be(AcceptanceCriteriaPriority.Low);
        dto.Notes.Should().BeNull();
    }

    [Fact]
    public void AcceptanceCriteriaDto_ShouldSetPropertiesCorrectly()
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
        var dto = new AcceptanceCriteriaDto
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
        dto.AcceptanceCriteriaId.Should().Be(id);
        dto.Given.Should().Be(given);
        dto.When.Should().Be(when);
        dto.Then.Should().Be(then);
        dto.Status.Should().Be(status);
        dto.Priority.Should().Be(priority);
        dto.Notes.Should().Be(notes);
        dto.CreatedAt.Should().Be(createdAt);
        dto.UpdatedAt.Should().Be(updatedAt);
    }
}
