// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Services;

public class ResponseBaseTests
{
    [Fact]
    public void ResponseBase_ShouldInitializeWithEmptyErrorsList()
    {
        // Arrange & Act
        var response = new ResponseBase();

        // Assert
        response.Errors.Should().NotBeNull();
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ResponseBase_ShouldAllowAddingErrors()
    {
        // Arrange
        var response = new ResponseBase();

        // Act
        response.Errors.Add("Error 1");
        response.Errors.Add("Error 2");

        // Assert
        response.Errors.Should().HaveCount(2);
        response.Errors.Should().Contain("Error 1");
        response.Errors.Should().Contain("Error 2");
    }

    [Fact]
    public void ResponseBase_ShouldAllowReplacingErrorsList()
    {
        // Arrange
        var response = new ResponseBase();
        var newErrors = new List<string> { "New Error 1", "New Error 2", "New Error 3" };

        // Act
        response.Errors = newErrors;

        // Assert
        response.Errors.Should().HaveCount(3);
        response.Errors.Should().BeEquivalentTo(newErrors);
    }

    [Fact]
    public void ResponseBase_ShouldAllowClearingErrors()
    {
        // Arrange
        var response = new ResponseBase();
        response.Errors.Add("Error 1");
        response.Errors.Add("Error 2");

        // Act
        response.Errors.Clear();

        // Assert
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ResponseBase_MultipleInstances_ShouldNotShareErrors()
    {
        // Arrange & Act
        var response1 = new ResponseBase();
        var response2 = new ResponseBase();

        response1.Errors.Add("Error for response 1");

        // Assert
        response1.Errors.Should().HaveCount(1);
        response2.Errors.Should().BeEmpty();
    }
}
