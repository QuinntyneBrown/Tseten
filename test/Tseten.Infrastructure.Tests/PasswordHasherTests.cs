// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Infrastructure;

namespace Tseten.Infrastructure.Tests;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void GenerateSalt_ShouldReturnNonEmptySalt()
    {
        // Act
        var salt = _passwordHasher.GenerateSalt();

        // Assert
        salt.Should().NotBeNull();
        salt.Should().NotBeEmpty();
        salt.Length.Should().Be(16); // SaltSize = 16
    }

    [Fact]
    public void GenerateSalt_ShouldReturnUniqueSalts()
    {
        // Act
        var salt1 = _passwordHasher.GenerateSalt();
        var salt2 = _passwordHasher.GenerateSalt();

        // Assert
        salt1.Should().NotBeEquivalentTo(salt2);
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyHash()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt = _passwordHasher.GenerateSalt();

        // Act
        var hash = _passwordHasher.HashPassword(password, salt);

        // Assert
        hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void HashPassword_ShouldReturnConsistentHashForSameSalt()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt = _passwordHasher.GenerateSalt();

        // Act
        var hash1 = _passwordHasher.HashPassword(password, salt);
        var hash2 = _passwordHasher.HashPassword(password, salt);

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashForDifferentSalt()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt1 = _passwordHasher.GenerateSalt();
        var salt2 = _passwordHasher.GenerateSalt();

        // Act
        var hash1 = _passwordHasher.HashPassword(password, salt1);
        var hash2 = _passwordHasher.HashPassword(password, salt2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashForDifferentPassword()
    {
        // Arrange
        var password1 = "TestPassword123!";
        var password2 = "DifferentPassword456!";
        var salt = _passwordHasher.GenerateSalt();

        // Act
        var hash1 = _passwordHasher.HashPassword(password1, salt);
        var hash2 = _passwordHasher.HashPassword(password2, salt);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt = _passwordHasher.GenerateSalt();
        var hash = _passwordHasher.HashPassword(password, salt);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hash, salt);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
    {
        // Arrange
        var correctPassword = "TestPassword123!";
        var incorrectPassword = "WrongPassword456!";
        var salt = _passwordHasher.GenerateSalt();
        var hash = _passwordHasher.HashPassword(correctPassword, salt);

        // Act
        var result = _passwordHasher.VerifyPassword(incorrectPassword, hash, salt);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForWrongSalt()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt1 = _passwordHasher.GenerateSalt();
        var salt2 = _passwordHasher.GenerateSalt();
        var hash = _passwordHasher.HashPassword(password, salt1);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hash, salt2);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("short")]
    [InlineData("VeryLongPasswordWithManyCharacters!@#$%^&*()1234567890")]
    public void HashPassword_ShouldWorkWithVariousPasswordLengths(string password)
    {
        // Arrange
        var salt = _passwordHasher.GenerateSalt();

        // Act
        var hash = _passwordHasher.HashPassword(password, salt);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordHasher.VerifyPassword(password, hash, salt).Should().BeTrue();
    }

    [Fact]
    public void HashPassword_ShouldProduceBase64EncodedHash()
    {
        // Arrange
        var password = "TestPassword123!";
        var salt = _passwordHasher.GenerateSalt();

        // Act
        var hash = _passwordHasher.HashPassword(password, salt);

        // Assert - Base64 strings should be decodable without exception
        Action decodeAction = () => Convert.FromBase64String(hash);
        decodeAction.Should().NotThrow();
    }
}
