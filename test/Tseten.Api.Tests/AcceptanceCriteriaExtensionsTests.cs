// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Tests;

public class AcceptanceCriteriaExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddHours(1);

        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = id,
            Given = "Given a user is logged in",
            When = "When the user clicks save",
            Then = "Then data is persisted",
            Status = AcceptanceCriteriaStatus.Passed,
            Priority = AcceptanceCriteriaPriority.High,
            Notes = "Test notes",
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act
        var dto = acceptanceCriteria.ToDto();

        // Assert
        dto.AcceptanceCriteriaId.Should().Be(id);
        dto.Given.Should().Be("Given a user is logged in");
        dto.When.Should().Be("When the user clicks save");
        dto.Then.Should().Be("Then data is persisted");
        dto.Status.Should().Be(AcceptanceCriteriaStatus.Passed);
        dto.Priority.Should().Be(AcceptanceCriteriaPriority.High);
        dto.Notes.Should().Be("Test notes");
        dto.CreatedAt.Should().Be(createdAt);
        dto.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void ToDto_ListExtension_ShouldMapAllItems()
    {
        // Arrange
        var list = new List<AcceptanceCriteria>
        {
            new()
            {
                AcceptanceCriteriaId = Guid.NewGuid(),
                Given = "Given 1",
                When = "When 1",
                Then = "Then 1",
                Status = AcceptanceCriteriaStatus.Pending,
                Priority = AcceptanceCriteriaPriority.Low
            },
            new()
            {
                AcceptanceCriteriaId = Guid.NewGuid(),
                Given = "Given 2",
                When = "When 2",
                Then = "Then 2",
                Status = AcceptanceCriteriaStatus.Passed,
                Priority = AcceptanceCriteriaPriority.High
            }
        };

        // Act
        var dtos = list.ToDto();

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].Given.Should().Be("Given 1");
        dtos[1].Given.Should().Be("Given 2");
    }

    [Fact]
    public void ToDto_ListExtension_ShouldReturnEmptyListForNull()
    {
        // Arrange
        List<AcceptanceCriteria>? list = null;

        // Act
        var dtos = list.ToDto();

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_ListExtension_ShouldReturnEmptyListForEmptyList()
    {
        // Arrange
        var list = new List<AcceptanceCriteria>();

        // Act
        var dtos = list.ToDto();

        // Assert
        dtos.Should().NotBeNull();
        dtos.Should().BeEmpty();
    }
}
