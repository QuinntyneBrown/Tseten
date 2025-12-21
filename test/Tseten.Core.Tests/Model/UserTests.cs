// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Model;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveDefaultEmptyRolesList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Roles.Should().NotBeNull();
        user.Roles.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldHaveDefaultEmptyProfilesList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Profiles.Should().NotBeNull();
        user.Profiles.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldHaveDefaultEmptySalt()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Salt.Should().NotBeNull();
        user.Salt.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldHaveDefaultEmptyUsername()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Username.Should().Be(string.Empty);
    }

    [Fact]
    public void User_ShouldHaveDefaultEmptyPassword()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Password.Should().Be(string.Empty);
    }

    [Fact]
    public void User_ShouldHaveDefaultFalseIsDeleted()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldHaveDefaultNullProfileIds()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.CurrentProfileId.Should().BeNull();
        user.DefaultProfileId.Should().BeNull();
    }

    [Fact]
    public void User_ShouldAllowSettingProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var salt = new byte[] { 1, 2, 3, 4 };

        // Act
        var user = new User
        {
            UserId = userId,
            Username = "testuser",
            Password = "hashedpassword",
            Salt = salt,
            CurrentProfileId = profileId,
            DefaultProfileId = profileId,
            IsDeleted = true
        };

        // Assert
        user.UserId.Should().Be(userId);
        user.Username.Should().Be("testuser");
        user.Password.Should().Be("hashedpassword");
        user.Salt.Should().BeEquivalentTo(salt);
        user.CurrentProfileId.Should().Be(profileId);
        user.DefaultProfileId.Should().Be(profileId);
        user.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldAllowAddingRoles()
    {
        // Arrange
        var user = new User { UserId = Guid.NewGuid(), Username = "testuser" };
        var role = new Role { RoleId = Guid.NewGuid(), Name = "Admin" };

        // Act
        user.Roles.Add(role);

        // Assert
        user.Roles.Should().HaveCount(1);
        user.Roles[0].Name.Should().Be("Admin");
    }

    [Fact]
    public void User_ShouldAllowAddingProfiles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId, Username = "testuser" };
        var profile = new Profile
        {
            ProfileId = Guid.NewGuid(),
            UserId = userId,
            Firstname = "John",
            Lastname = "Doe"
        };

        // Act
        user.Profiles.Add(profile);

        // Assert
        user.Profiles.Should().HaveCount(1);
        user.Profiles[0].Firstname.Should().Be("John");
    }
}
