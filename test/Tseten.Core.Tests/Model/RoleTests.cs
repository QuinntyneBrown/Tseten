// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Model;

public class RoleTests
{
    [Fact]
    public void Role_ShouldHaveDefaultEmptyPrivilegesList()
    {
        // Arrange & Act
        var role = new Role();

        // Assert
        role.Privileges.Should().NotBeNull();
        role.Privileges.Should().BeEmpty();
    }

    [Fact]
    public void Role_ShouldHaveDefaultEmptyUsersList()
    {
        // Arrange & Act
        var role = new Role();

        // Assert
        role.Users.Should().NotBeNull();
        role.Users.Should().BeEmpty();
    }

    [Fact]
    public void Role_ShouldHaveDefaultEmptyName()
    {
        // Arrange & Act
        var role = new Role();

        // Assert
        role.Name.Should().Be(string.Empty);
    }

    [Fact]
    public void Role_ShouldAllowSettingProperties()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        // Act
        var role = new Role
        {
            RoleId = roleId,
            Name = "SystemAdministrator"
        };

        // Assert
        role.RoleId.Should().Be(roleId);
        role.Name.Should().Be("SystemAdministrator");
    }

    [Fact]
    public void Role_ShouldAllowAddingPrivileges()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "Admin" };
        var privilege = new Privilege
        {
            PrivilegeId = Guid.NewGuid(),
            RoleId = roleId,
            Aggregate = "User",
            AccessRight = AccessRight.Read
        };

        // Act
        role.Privileges.Add(privilege);

        // Assert
        role.Privileges.Should().HaveCount(1);
        role.Privileges[0].Aggregate.Should().Be("User");
        role.Privileges[0].AccessRight.Should().Be(AccessRight.Read);
    }

    [Fact]
    public void Role_ShouldAllowMultiplePrivileges()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "Admin" };

        // Act
        role.Privileges.Add(new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = roleId, Aggregate = "User", AccessRight = AccessRight.Read });
        role.Privileges.Add(new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = roleId, Aggregate = "User", AccessRight = AccessRight.Write });
        role.Privileges.Add(new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = roleId, Aggregate = "User", AccessRight = AccessRight.Create });
        role.Privileges.Add(new Privilege { PrivilegeId = Guid.NewGuid(), RoleId = roleId, Aggregate = "User", AccessRight = AccessRight.Delete });

        // Assert
        role.Privileges.Should().HaveCount(4);
    }
}
