// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Tseten.Api;
using Tseten.Core;

namespace Tseten.Api.Tests.Extensions;

public class TagExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new Tag
        {
            TagId = tagId,
            Name = "Feature",
            Description = "This is a feature tag"
        };

        // Act
        var dto = tag.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.TagId.Should().Be(tagId);
        dto.Name.Should().Be("Feature");
        dto.Description.Should().Be("This is a feature tag");
    }

    [Fact]
    public void ToDto_WithEmptyStrings_ShouldMapCorrectly()
    {
        // Arrange
        var tag = new Tag
        {
            TagId = Guid.NewGuid(),
            Name = "",
            Description = ""
        };

        // Act
        var dto = tag.ToDto();

        // Assert
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_WithDefaultValues_ShouldMapCorrectly()
    {
        // Arrange
        var tag = new Tag();

        // Act
        var dto = tag.ToDto();

        // Assert
        dto.TagId.Should().Be(Guid.Empty);
        dto.Name.Should().Be(string.Empty);
        dto.Description.Should().Be(string.Empty);
    }
}
