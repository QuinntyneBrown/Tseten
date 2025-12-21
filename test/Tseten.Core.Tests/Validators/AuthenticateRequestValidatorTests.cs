// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Validators;

public class AuthenticateRequestValidatorTests
{
    private readonly AuthenticateRequestValidator _validator;

    public AuthenticateRequestValidatorTests()
    {
        _validator = new AuthenticateRequestValidator();
    }

    [Fact]
    public async Task Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            Username = "admin@example.com",
            Password = "SecurePassword123!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyUsername_ShouldFail()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            Username = "",
            Password = "SecurePassword123!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Username");
        result.Errors.Should().Contain(e => e.ErrorMessage == "Username is required");
    }

    [Fact]
    public async Task Validate_WithEmptyPassword_ShouldFail()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            Username = "admin@example.com",
            Password = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
        result.Errors.Should().Contain(e => e.ErrorMessage == "Password is required");
    }

    [Fact]
    public async Task Validate_WithBothEmpty_ShouldFailWithTwoErrors()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            Username = "",
            Password = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("user", "pass")]
    [InlineData("admin@tseten.com", "Admin123!")]
    [InlineData("a", "b")]
    public async Task Validate_WithVariousValidInputs_ShouldPass(string username, string password)
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
