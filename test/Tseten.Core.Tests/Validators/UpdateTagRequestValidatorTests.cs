// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Validators;

public class UpdateTagRequestValidatorTests
{
    private readonly UpdateTagRequestValidator _validator;

    public UpdateTagRequestValidatorTests()
    {
        _validator = new UpdateTagRequestValidator();
    }

    [Fact]
    public async Task Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        var request = new UpdateTagRequest
        {
            TagId = Guid.NewGuid(),
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
    public async Task Validate_WithEmptyGuidTagId_ShouldPass()
    {
        // Arrange - Note: NotNull allows empty Guid since Guid is a value type
        var request = new UpdateTagRequest
        {
            TagId = Guid.Empty,
            Name = "Feature",
            Description = "This is a feature tag"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        // Guid.Empty is not null, so it passes NotNull() validation
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var request = new UpdateTagRequest
        {
            TagId = Guid.NewGuid(),
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
    public async Task Validate_WithEmptyDescription_ShouldFail()
    {
        // Arrange
        var request = new UpdateTagRequest
        {
            TagId = Guid.NewGuid(),
            Name = "Feature",
            Description = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }
}
