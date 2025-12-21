// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Models.SoftwareRequirement;

namespace Tseten.Core.Tests.SoftwareRequirement;

public class AcceptanceCriteriaValidatorTests
{
    private readonly AcceptanceCriteriaValidator _validator;

    public AcceptanceCriteriaValidatorTests()
    {
        _validator = new AcceptanceCriteriaValidator();
    }

    [Fact]
    public async Task Validate_WithValidAcceptanceCriteria_ShouldPass()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a user is logged in",
            When = "When the user clicks the submit button",
            Then = "Then the form is submitted successfully",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.High
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyGiven_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "",
            When = "When the user clicks the submit button",
            Then = "Then the form is submitted successfully",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.High
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Given");
        result.Errors.Should().Contain(e => e.ErrorMessage == "Given is required");
    }

    [Fact]
    public async Task Validate_WithEmptyWhen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a user is logged in",
            When = "",
            Then = "Then the form is submitted successfully",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.High
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "When");
        result.Errors.Should().Contain(e => e.ErrorMessage == "When is required");
    }

    [Fact]
    public async Task Validate_WithEmptyThen_ShouldFail()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a user is logged in",
            When = "When the user clicks the submit button",
            Then = "",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = AcceptanceCriteriaPriority.High
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Then");
        result.Errors.Should().Contain(e => e.ErrorMessage == "Then is required");
    }

    [Theory]
    [InlineData(AcceptanceCriteriaStatus.Pending)]
    [InlineData(AcceptanceCriteriaStatus.Passed)]
    [InlineData(AcceptanceCriteriaStatus.Failed)]
    [InlineData(AcceptanceCriteriaStatus.NotApplicable)]
    public async Task Validate_WithValidStatus_ShouldPass(AcceptanceCriteriaStatus status)
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a valid condition",
            When = "When an action is performed",
            Then = "Then expected result occurs",
            Status = status,
            Priority = AcceptanceCriteriaPriority.Medium
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(AcceptanceCriteriaPriority.Low)]
    [InlineData(AcceptanceCriteriaPriority.Medium)]
    [InlineData(AcceptanceCriteriaPriority.High)]
    public async Task Validate_WithValidPriority_ShouldPass(AcceptanceCriteriaPriority priority)
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a valid condition",
            When = "When an action is performed",
            Then = "Then expected result occurs",
            Status = AcceptanceCriteriaStatus.Pending,
            Priority = priority
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithAllEmptyFields_ShouldFailWithMultipleErrors()
    {
        // Arrange
        var acceptanceCriteria = new AcceptanceCriteria
        {
            Given = "",
            When = "",
            Then = ""
        };

        // Act
        var result = await _validator.ValidateAsync(acceptanceCriteria);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(3);
    }
}
