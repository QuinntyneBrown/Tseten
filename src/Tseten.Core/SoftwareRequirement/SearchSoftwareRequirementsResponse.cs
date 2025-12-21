// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Response for paginated semantic search of software requirements.
/// </summary>
public class SearchSoftwareRequirementsResponse
{
    /// <summary>
    /// The search results with similarity scores.
    /// </summary>
    public List<SearchResultDto> Results { get; set; } = [];

    /// <summary>
    /// The current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The total number of matching results.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Whether there is a next page.
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// The search query that was used.
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Any errors that occurred during the search.
    /// </summary>
    public List<string> Errors { get; set; } = [];
}

/// <summary>
/// Individual search result with similarity score.
/// </summary>
public class SearchResultDto
{
    /// <summary>
    /// The software requirement data.
    /// </summary>
    public SoftwareRequirementDto SoftwareRequirement { get; set; } = null!;

    /// <summary>
    /// The similarity score (0.0 to 1.0).
    /// </summary>
    public float SimilarityScore { get; set; }

    /// <summary>
    /// A snippet or highlight of the matching text.
    /// </summary>
    public string? Highlight { get; set; }
}
