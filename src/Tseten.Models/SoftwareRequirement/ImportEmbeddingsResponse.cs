// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Response from the import embeddings operation.
/// </summary>
public class ImportEmbeddingsResponse
{
    /// <summary>
    /// Whether the import was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The number of software requirements imported.
    /// </summary>
    public int ImportedCount { get; set; }

    /// <summary>
    /// The timestamp when the import completed.
    /// </summary>
    public DateTime CompletedAt { get; set; }

    /// <summary>
    /// Any errors that occurred during import.
    /// </summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>
    /// Additional message or summary.
    /// </summary>
    public string? Message { get; set; }
}
