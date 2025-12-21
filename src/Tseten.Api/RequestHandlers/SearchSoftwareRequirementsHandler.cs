// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;
using Tseten.Api.Services;

namespace Tseten.Api.RequestHandlers;

/// <summary>
/// Handler for paginated semantic search of software requirements.
/// Uses vector similarity search against SQL Express with cosine similarity.
/// </summary>
public class SearchSoftwareRequirementsHandler : IRequestHandler<SearchSoftwareRequirementsRequest, SearchSoftwareRequirementsResponse>
{
    private readonly ILogger<SearchSoftwareRequirementsHandler> _logger;
    private readonly ITsetenContext _context;
    private readonly IEmbeddingService _embeddingService;

    public SearchSoftwareRequirementsHandler(
        ILogger<SearchSoftwareRequirementsHandler> logger,
        ITsetenContext context,
        IEmbeddingService embeddingService)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(embeddingService);

        _logger = logger;
        _context = context;
        _embeddingService = embeddingService;
    }

    public async Task<SearchSoftwareRequirementsResponse> Handle(SearchSoftwareRequirementsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Searching software requirements with query: {Query}, Page: {Page}, PageSize: {PageSize}",
            request.Query, request.Page, request.PageSize);

        try
        {
            // Generate embedding for the search query
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(request.Query, cancellationToken);

            // Get all embeddings from the database
            var allEmbeddings = await _context.SoftwareRequirementEmbeddings
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Calculate similarity scores for all embeddings
            var scoredResults = allEmbeddings
                .Select(embedding => new
                {
                    Embedding = embedding,
                    Similarity = _embeddingService.CalculateCosineSimilarity(queryEmbedding, embedding.Embedding)
                })
                .Where(x => x.Similarity >= request.MinSimilarity)
                .OrderByDescending(x => x.Similarity)
                .ToList();

            // Get all software requirements to join with embeddings
            var allRequirements = await _context.SoftwareRequirements
                .Include(sr => sr.Comments)
                .Include(sr => sr.AcceptanceCriteria)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var requirementsDict = allRequirements.ToDictionary(r => r.SoftwareRequirementId);

            // Apply additional filters
            var filteredResults = scoredResults
                .Where(x => requirementsDict.ContainsKey(x.Embedding.SoftwareRequirementId))
                .Where(x =>
                {
                    var req = requirementsDict[x.Embedding.SoftwareRequirementId];
                    if (request.CanImplement.HasValue && req.CanImplement != request.CanImplement.Value)
                        return false;
                    if (request.CanTest.HasValue && req.CanTest != request.CanTest.Value)
                        return false;
                    return true;
                })
                .ToList();

            // Calculate pagination
            var totalCount = filteredResults.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
            var skip = (request.Page - 1) * request.PageSize;

            // Get page of results
            var pagedResults = filteredResults
                .Skip(skip)
                .Take(request.PageSize)
                .ToList();

            // Build response
            var results = pagedResults
                .Select(x =>
                {
                    var requirement = requirementsDict[x.Embedding.SoftwareRequirementId];
                    return new SearchResultDto
                    {
                        SoftwareRequirement = requirement.ToDto(),
                        SimilarityScore = x.Similarity,
                        Highlight = GenerateHighlight(requirement.Description, request.Query)
                    };
                })
                .ToList();

            _logger.LogInformation(
                "Search completed. Found {TotalCount} results, returning page {Page} of {TotalPages}",
                totalCount, request.Page, totalPages);

            return new SearchSoftwareRequirementsResponse
            {
                Results = results,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = request.Page < totalPages,
                HasPreviousPage = request.Page > 1,
                Query = request.Query
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching software requirements");

            return new SearchSoftwareRequirementsResponse
            {
                Query = request.Query,
                Page = request.Page,
                PageSize = request.PageSize,
                Errors = [$"Search failed: {ex.Message}"]
            };
        }
    }

    private static string? GenerateHighlight(string description, string query)
    {
        if (string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(query))
        {
            return null;
        }

        // Find the most relevant snippet containing query terms
        var queryTerms = query.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var descriptionLower = description.ToLowerInvariant();

        // Find first matching term position
        var firstMatchIndex = -1;
        foreach (var term in queryTerms)
        {
            var index = descriptionLower.IndexOf(term, StringComparison.OrdinalIgnoreCase);
            if (index >= 0 && (firstMatchIndex < 0 || index < firstMatchIndex))
            {
                firstMatchIndex = index;
            }
        }

        if (firstMatchIndex < 0)
        {
            // No direct match, return beginning of description
            return description.Length > 150 ? description[..150] + "..." : description;
        }

        // Extract a snippet around the match
        var start = Math.Max(0, firstMatchIndex - 50);
        var end = Math.Min(description.Length, firstMatchIndex + 100);

        var highlight = description[start..end];

        if (start > 0) highlight = "..." + highlight;
        if (end < description.Length) highlight += "...";

        return highlight;
    }
}
