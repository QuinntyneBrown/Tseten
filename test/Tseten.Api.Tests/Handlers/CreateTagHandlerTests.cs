// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tseten.Api.Features.Tag;
using Tseten.Core;

namespace Tseten.Api.Tests.Handlers;

public class CreateTagHandlerTests
{
    private readonly Mock<ITsetenContext> _mockContext;
    private readonly Mock<ILogger<CreateTagHandler>> _mockLogger;
    private readonly Mock<DbSet<Tag>> _mockTagsDbSet;
    private readonly CreateTagHandler _handler;

    public CreateTagHandlerTests()
    {
        _mockContext = new Mock<ITsetenContext>();
        _mockLogger = new Mock<ILogger<CreateTagHandler>>();
        _mockTagsDbSet = new Mock<DbSet<Tag>>();

        _mockContext.Setup(c => c.Tags).Returns(_mockTagsDbSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateTagHandler(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateTag()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "Feature",
            Description = "Feature tag description"
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Tag.Should().NotBeNull();
        response.Tag!.Name.Should().Be("Feature");
        response.Tag.Description.Should().Be("Feature tag description");
        response.Tag.TagId.Should().NotBe(Guid.Empty);

        _mockTagsDbSet.Verify(s => s.Add(It.IsAny<Tag>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldGenerateNewTagId()
    {
        // Arrange
        var request = new CreateTagRequest
        {
            Name = "Bug",
            Description = "Bug tag"
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Tag!.TagId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrow()
    {
        // Act & Assert
        Action act = () => new CreateTagHandler(null!, _mockLogger.Object);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrow()
    {
        // Act & Assert
        Action act = () => new CreateTagHandler(_mockContext.Object, null!);
        act.Should().Throw<ArgumentNullException>();
    }
}
