// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Models.SoftwareRequirement;

public class Comment
{
    public Guid CommentId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public string Body { get; set; }
    public string Author { get; set; }
    public bool Resolved { get; set; }
    public Comment? ParentComment { get; set; }
    public List<Comment> Comments { get; set; }
}

