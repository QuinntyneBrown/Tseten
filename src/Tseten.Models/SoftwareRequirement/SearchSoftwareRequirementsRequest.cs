// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Request for paginated semantic search of software requirements.
/// </summary>
public class SearchSoftwareRequirementsRequest : IRequest<SearchSoftwareRequirementsResponse>
{
    /// <summary>
    /// The search query text for semantic similarity search.
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// The page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Minimum similarity score threshold (0.0 to 1.0).
    /// Results with similarity below this threshold will be excluded.
    /// </summary>
    public float MinSimilarity { get; set; } = 0.0f;

    /// <summary>
    /// Optional filter by CanImplement flag.
    /// </summary>
    public bool? CanImplement { get; set; }

    /// <summary>
    /// Optional filter by CanTest flag.
    /// </summary>
    public bool? CanTest { get; set; }
}
