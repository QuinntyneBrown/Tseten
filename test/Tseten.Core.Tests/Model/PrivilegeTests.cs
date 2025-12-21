// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core;

namespace Tseten.Core.Tests.Model;

public class PrivilegeTests
{
    [Fact]
    public void Privilege_ShouldHaveDefaultEmptyAggregate()
    {
        // Arrange & Act
        var privilege = new Privilege();

        // Assert
        privilege.Aggregate.Should().Be(string.Empty);
    }

    [Fact]
    public void Privilege_ShouldHaveDefaultNoneAccessRight()
    {
        // Arrange & Act
        var privilege = new Privilege();

        // Assert
        privilege.AccessRight.Should().Be(AccessRight.None);
    }

    [Fact]
    public void Privilege_ShouldHaveDefaultNullRole()
    {
        // Arrange & Act
        var privilege = new Privilege();

        // Assert
        privilege.Role.Should().BeNull();
    }

    [Fact]
    public void Privilege_ShouldAllowSettingProperties()
    {
        // Arrange
        var privilegeId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "Admin" };

        // Act
        var privilege = new Privilege
        {
            PrivilegeId = privilegeId,
            RoleId = roleId,
            Aggregate = "SoftwareRequirement",
            AccessRight = AccessRight.Write,
            Role = role
        };

        // Assert
        privilege.PrivilegeId.Should().Be(privilegeId);
        privilege.RoleId.Should().Be(roleId);
        privilege.Aggregate.Should().Be("SoftwareRequirement");
        privilege.AccessRight.Should().Be(AccessRight.Write);
        privilege.Role.Should().Be(role);
    }

    [Theory]
    [InlineData(AccessRight.None, 0)]
    [InlineData(AccessRight.Read, 1)]
    [InlineData(AccessRight.Write, 2)]
    [InlineData(AccessRight.Create, 3)]
    [InlineData(AccessRight.Delete, 4)]
    public void AccessRight_ShouldHaveCorrectValues(AccessRight accessRight, int expectedValue)
    {
        // Assert
        ((int)accessRight).Should().Be(expectedValue);
    }
}
