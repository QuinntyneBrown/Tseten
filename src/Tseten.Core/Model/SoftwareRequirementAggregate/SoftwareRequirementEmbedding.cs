// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tseten.Core;

[Index(nameof(SoftwareRequirementId), IsUnique = true)]
public class SoftwareRequirementEmbedding
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string SoftwareRequirementId { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string EmbeddingVector { get; set; } = string.Empty;

    public int EmbeddingDimension { get; set; } = 384;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public float[] Embedding
    {
        get => string.IsNullOrEmpty(EmbeddingVector)
            ? []
            : EmbeddingVector.Split(',').Select(float.Parse).ToArray();
        set => EmbeddingVector = string.Join(",", value.Select(v => v.ToString("G9")));
    }
}
