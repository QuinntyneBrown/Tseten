// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Model;

public class TagTests
{
    [Fact]
    public void Tag_ShouldHaveDefaultEmptyName()
    {
        // Arrange & Act
        var tag = new Tag();

        // Assert
        tag.Name.Should().Be(string.Empty);
    }

    [Fact]
    public void Tag_ShouldHaveDefaultEmptyDescription()
    {
        // Arrange & Act
        var tag = new Tag();

        // Assert
        tag.Description.Should().Be(string.Empty);
    }

    [Fact]
    public void Tag_ShouldAllowSettingProperties()
    {
        // Arrange
        var tagId = Guid.NewGuid();

        // Act
        var tag = new Tag
        {
            TagId = tagId,
            Name = "Feature",
            Description = "This tag represents a feature requirement"
        };

        // Assert
        tag.TagId.Should().Be(tagId);
        tag.Name.Should().Be("Feature");
        tag.Description.Should().Be("This tag represents a feature requirement");
    }

    [Fact]
    public void Tag_ShouldHaveDefaultGuidTagId()
    {
        // Arrange & Act
        var tag = new Tag();

        // Assert
        tag.TagId.Should().Be(Guid.Empty);
    }
}
