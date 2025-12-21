// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Entity representing a software requirement with its vector embedding for semantic search.
/// Uses SQL Server vector support for efficient similarity search.
/// </summary>
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

    /// <summary>
    /// Vector embedding for semantic search.
    /// Stored as a comma-separated string of float values for SQL Express compatibility.
    /// Standard embedding dimensions: 384 (MiniLM), 768 (BERT), 1536 (OpenAI ada-002)
    /// </summary>
    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string EmbeddingVector { get; set; } = string.Empty;

    /// <summary>
    /// Dimension of the embedding vector.
    /// </summary>
    public int EmbeddingDimension { get; set; } = 384;

    /// <summary>
    /// Timestamp when the embedding was created or last updated.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the embedding was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Helper property to get embedding as float array.
    /// </summary>
    [NotMapped]
    public float[] Embedding
    {
        get => string.IsNullOrEmpty(EmbeddingVector)
            ? []
            : EmbeddingVector.Split(',').Select(float.Parse).ToArray();
        set => EmbeddingVector = string.Join(",", value.Select(v => v.ToString("G9")));
    }
}
