// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Models.SoftwareRequirement;

namespace Tseten.Core.Tests.SoftwareRequirement;

public class CommentTests
{
    [Fact]
    public void Comment_ShouldHaveDefaultResolvedFalse()
    {
        // Arrange & Act
        var comment = new Comment();

        // Assert
        comment.Resolved.Should().BeFalse();
    }

    [Fact]
    public void Comment_ShouldHaveDefaultNullParentCommentId()
    {
        // Arrange & Act
        var comment = new Comment();

        // Assert
        comment.ParentCommentId.Should().BeNull();
    }

    [Fact]
    public void Comment_ShouldHaveDefaultNullParentComment()
    {
        // Arrange & Act
        var comment = new Comment();

        // Assert
        comment.ParentComment.Should().BeNull();
    }

    [Fact]
    public void Comment_ShouldAllowSettingAllProperties()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var parentCommentId = Guid.NewGuid();

        // Act
        var comment = new Comment
        {
            CommentId = commentId,
            ParentCommentId = parentCommentId,
            Body = "This is a test comment",
            Author = "testuser",
            Resolved = true
        };

        // Assert
        comment.CommentId.Should().Be(commentId);
        comment.ParentCommentId.Should().Be(parentCommentId);
        comment.Body.Should().Be("This is a test comment");
        comment.Author.Should().Be("testuser");
        comment.Resolved.Should().BeTrue();
    }

    [Fact]
    public void Comment_ShouldAllowNestedReplies()
    {
        // Arrange
        var parentComment = new Comment
        {
            CommentId = Guid.NewGuid(),
            Body = "Parent comment",
            Author = "user1",
            Resolved = false,
            Comments = new List<Comment>()
        };

        var replyComment = new Comment
        {
            CommentId = Guid.NewGuid(),
            ParentCommentId = parentComment.CommentId,
            ParentComment = parentComment,
            Body = "Reply to parent",
            Author = "user2",
            Resolved = false
        };

        // Act
        parentComment.Comments.Add(replyComment);

        // Assert
        parentComment.Comments.Should().HaveCount(1);
        parentComment.Comments[0].Body.Should().Be("Reply to parent");
        parentComment.Comments[0].ParentComment.Should().Be(parentComment);
    }
}
