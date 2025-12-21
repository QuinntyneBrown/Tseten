// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using SpecSync.Models.SoftwareRequirement;

namespace SpecSync.SoftwareRequirements.Api.Tests;

public class AcceptanceCriteriaValidatorTests
{
    private readonly AcceptanceCriteriaValidator _validator;

    public AcceptanceCriteriaValidatorTests()
    {
        _validator = new AcceptanceCriteriaValidator();
    }

    [Fact]
    public void Validate_ValidAcceptanceCriteria_ShouldPass()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a user is logged in",
            When = "When the user clicks save",
            Then = "Then data is persisted",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.Medium
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_EmptyGiven_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "",
            When = "When action",
            Then = "Then result"
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Given");
    }

    [Fact]
    public void Validate_NullGiven_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = null!,
            When = "When action",
            Then = "Then result"
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Given");
    }

    [Fact]
    public void Validate_EmptyWhen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "Given context",
            When = "",
            Then = "Then result"
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "When");
    }

    [Fact]
    public void Validate_NullWhen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "Given context",
            When = null!,
            Then = "Then result"
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "When");
    }

    [Fact]
    public void Validate_EmptyThen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "Given context",
            When = "When action",
            Then = ""
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Then");
    }

    [Fact]
    public void Validate_NullThen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "Given context",
            When = "When action",
            Then = null!
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Then");
    }

    [Fact]
    public void Validate_AllFieldsEmpty_ShouldReturnMultipleErrors()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "",
            When = "",
            Then = ""
        };

        // Act
        var result = _validator.Validate(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterOrEqualTo(3);
    }
}
