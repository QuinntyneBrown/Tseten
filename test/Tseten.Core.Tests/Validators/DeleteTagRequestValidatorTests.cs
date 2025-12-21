// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Validators;

public class DeleteTagRequestValidatorTests
{
    private readonly DeleteTagRequestValidator _validator;

    public DeleteTagRequestValidatorTests()
    {
        _validator = new DeleteTagRequestValidator();
    }

    [Fact]
    public async Task Validate_WithValidTagId_ShouldPass()
    {
        // Arrange
        var request = new DeleteTagRequest
        {
            TagId = Guid.NewGuid()
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
        var request = new DeleteTagRequest
        {
            TagId = Guid.Empty
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        // Guid.Empty is not null, so it passes NotNull() validation
        result.IsValid.Should().BeTrue();
    }
}
