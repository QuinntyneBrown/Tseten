// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using SpecSync.Models.SoftwareRequirement;

namespace SpecSync.SoftwareRequirements.Api.Tests;

public class CreateSoftwareRequirementRequestValidatorTests
{
    private readonly CreateSoftwareRequirementRequestValidator _validator;

    public CreateSoftwareRequirementRequestValidatorTests()
    {
        _validator = new CreateSoftwareRequirementRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ShouldPass()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Valid requirement description",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria>
            {
                new()
                {
                    AcceptanceCriteriaId = Guid.NewGuid(),
                    Given = "Given context",
                    When = "When action",
                    Then = "Then result",
                    Status = AcceptanceCriteriaStatus.Pending,
                    Priority = AcceptanceCriteriaPriority.Medium
                }
            }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyDescription_ShouldFail()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "",
            CanImplement = true,
            CanTest = true
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_InvalidAcceptanceCriteria_ShouldFail()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Valid description",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria>
            {
                new()
                {
                    AcceptanceCriteriaId = Guid.NewGuid(),
                    Given = "", // Invalid - empty
                    When = "When action",
                    Then = "Then result"
                }
            }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("AcceptanceCriteria"));
    }

    [Fact]
    public void Validate_MultipleInvalidAcceptanceCriteria_ShouldReturnAllErrors()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Valid description",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria>
            {
                new()
                {
                    AcceptanceCriteriaId = Guid.NewGuid(),
                    Given = "",
                    When = "",
                    Then = ""
                },
                new()
                {
                    AcceptanceCriteriaId = Guid.NewGuid(),
                    Given = "",
                    When = "Valid when",
                    Then = "Valid then"
                }
            }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterOrEqualTo(4); // 3 from first AC, 1 from second
    }

    [Fact]
    public void Validate_NullAcceptanceCriteria_ShouldPass()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Valid description",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyAcceptanceCriteriaList_ShouldPass()
    {
        // Arrange
        var request = new CreateSoftwareRequirementRequest
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Valid description",
            CanImplement = true,
            CanTest = true,
            AcceptanceCriteria = new List<AcceptanceCriteria>()
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
