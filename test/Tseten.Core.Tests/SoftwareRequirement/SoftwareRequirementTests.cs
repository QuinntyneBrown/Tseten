// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using SR = Tseten.Models.SoftwareRequirement;

namespace Tseten.Core.Tests.SoftwareRequirement;

public class SoftwareRequirementTests
{
    [Fact]
    public void SoftwareRequirement_ShouldHaveDefaultEmptyCollections()
    {
        // Arrange & Act
        var requirement = new SR.SoftwareRequirement();

        // Assert
        requirement.Comments.Should().NotBeNull();
        requirement.Comments.Should().BeEmpty();
        requirement.AcceptanceCriteria.Should().NotBeNull();
        requirement.AcceptanceCriteria.Should().BeEmpty();
    }

    [Fact]
    public void SoftwareRequirement_ShouldAllowSettingAllProperties()
    {
        // Arrange & Act
        var requirement = new SR.SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            ParentSoftwareRequirementId = "REQ-000",
            Description = "Test requirement description",
            CanImplement = true,
            CanTest = true
        };

        // Assert
        requirement.SoftwareRequirementId.Should().Be("REQ-001");
        requirement.ParentSoftwareRequirementId.Should().Be("REQ-000");
        requirement.Description.Should().Be("Test requirement description");
        requirement.CanImplement.Should().BeTrue();
        requirement.CanTest.Should().BeTrue();
    }

    [Fact]
    public void SoftwareRequirement_ShouldAllowAddingAcceptanceCriteria()
    {
        // Arrange
        var requirement = new SR.SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Test requirement"
        };

        var criteria = new SR.AcceptanceCriteria
        {
            AcceptanceCriteriaId = Guid.NewGuid(),
            Given = "Given a condition",
            When = "When an action occurs",
            Then = "Then result is expected",
            Status = SR.AcceptanceCriteriaStatus.Pending,
            Priority = SR.AcceptanceCriteriaPriority.High
        };

        // Act
        requirement.AcceptanceCriteria.Add(criteria);

        // Assert
        requirement.AcceptanceCriteria.Should().HaveCount(1);
        requirement.AcceptanceCriteria[0].Given.Should().Be("Given a condition");
    }

    [Fact]
    public void SoftwareRequirement_ShouldAllowAddingComments()
    {
        // Arrange
        var requirement = new SR.SoftwareRequirement
        {
            SoftwareRequirementId = "REQ-001",
            Description = "Test requirement"
        };

        var comment = new SR.Comment
        {
            CommentId = Guid.NewGuid(),
            Body = "This is a comment",
            Author = "testuser",
            Resolved = false
        };

        // Act
        requirement.Comments.Add(comment);

        // Assert
        requirement.Comments.Should().HaveCount(1);
        requirement.Comments[0].Body.Should().Be("This is a comment");
    }

    [Fact]
    public void SoftwareRequirement_CanImplementAndCanTest_ShouldDefaultToFalse()
    {
        // Arrange & Act
        var requirement = new SR.SoftwareRequirement();

        // Assert
        requirement.CanImplement.Should().BeFalse();
        requirement.CanTest.Should().BeFalse();
    }
}
