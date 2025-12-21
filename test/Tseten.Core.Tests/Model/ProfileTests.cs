// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Model;

public class ProfileTests
{
    [Fact]
    public void Profile_ShouldHaveDefaultEmptyFirstname()
    {
        // Arrange & Act
        var profile = new Profile();

        // Assert
        profile.Firstname.Should().Be(string.Empty);
    }

    [Fact]
    public void Profile_ShouldHaveDefaultEmptyLastname()
    {
        // Arrange & Act
        var profile = new Profile();

        // Assert
        profile.Lastname.Should().Be(string.Empty);
    }

    [Fact]
    public void Profile_ShouldHaveDefaultNullPhoneNumber()
    {
        // Arrange & Act
        var profile = new Profile();

        // Assert
        profile.PhoneNumber.Should().BeNull();
    }

    [Fact]
    public void Profile_ShouldHaveDefaultNullUser()
    {
        // Arrange & Act
        var profile = new Profile();

        // Assert
        profile.User.Should().BeNull();
    }

    [Fact]
    public void Profile_ShouldHaveDefaultNullAvatarDigitalAssetId()
    {
        // Arrange & Act
        var profile = new Profile();

        // Assert
        profile.AvatarDigitalAssetId.Should().BeNull();
    }

    [Fact]
    public void Profile_ShouldAllowSettingProperties()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var avatarId = Guid.NewGuid();
        var user = new User { UserId = userId, Username = "testuser" };

        // Act
        var profile = new Profile
        {
            ProfileId = profileId,
            UserId = userId,
            Firstname = "John",
            Lastname = "Doe",
            AvatarDigitalAssetId = avatarId,
            PhoneNumber = "+1234567890",
            User = user
        };

        // Assert
        profile.ProfileId.Should().Be(profileId);
        profile.UserId.Should().Be(userId);
        profile.Firstname.Should().Be("John");
        profile.Lastname.Should().Be("Doe");
        profile.AvatarDigitalAssetId.Should().Be(avatarId);
        profile.PhoneNumber.Should().Be("+1234567890");
        profile.User.Should().Be(user);
    }
}
