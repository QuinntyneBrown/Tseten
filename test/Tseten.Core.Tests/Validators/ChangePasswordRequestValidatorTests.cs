// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Validators;

public class ChangePasswordRequestValidatorTests
{
    private readonly ChangePasswordRequestValidator _validator;

    public ChangePasswordRequestValidatorTests()
    {
        _validator = new ChangePasswordRequestValidator();
    }

    [Fact]
    public async Task Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword456!",
            ConfirmationPassword = "NewPassword456!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyOldPassword_ShouldFail()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "",
            NewPassword = "NewPassword456!",
            ConfirmationPassword = "NewPassword456!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OldPassword");
    }

    [Fact]
    public async Task Validate_WithEmptyNewPassword_ShouldFail()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "OldPassword123!",
            NewPassword = "",
            ConfirmationPassword = ""
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NewPassword");
    }

    [Fact]
    public async Task Validate_WithNewPasswordSameAsOld_ShouldFail()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "SamePassword123!",
            NewPassword = "SamePassword123!",
            ConfirmationPassword = "SamePassword123!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "New password must be different from old password");
    }

    [Fact]
    public async Task Validate_WithMismatchedConfirmationPassword_ShouldFail()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword456!",
            ConfirmationPassword = "DifferentPassword789!"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Password confirmation must match new password");
    }

    [Fact]
    public async Task Validate_WithShortNewPassword_ShouldFail()
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "OldPassword123!",
            NewPassword = "12345",
            ConfirmationPassword = "12345"
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NewPassword");
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("abcdef")]
    [InlineData("Password1")]
    public async Task Validate_WithMinimumLengthNewPassword_ShouldPass(string newPassword)
    {
        // Arrange
        var request = new ChangePasswordRequest
        {
            OldPassword = "OldPassword123!",
            NewPassword = newPassword,
            ConfirmationPassword = newPassword
        };

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
