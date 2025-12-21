// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Validators;

public class CreateTagRequestValidatorTests
{
    private readonly CreateTagRequestValidator _validator;

    public CreateTagRequestValidatorTests()
    {
        _validator = new CreateTagRequestValidator();
    }

    [Fact]
    public async Task Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "Feature",
            Description = "This is a feature tag"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "",
            Description = "This is a feature tag"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WithNullName_ShouldFail()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = null!,
            Description = "This is a feature tag"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WithEmptyDescription_ShouldFail()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "Feature",
            Description = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validate_WithNullDescription_ShouldFail()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "Feature",
            Description = null!
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validate_WithBothEmpty_ShouldFailWithTwoErrors()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "",
            Description = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
