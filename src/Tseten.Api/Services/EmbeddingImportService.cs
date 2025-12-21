// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Services;

/// <summary>
/// Service for importing and managing software requirement embeddings in the vector database.
/// </summary>
public class EmbeddingImportService : IEmbeddingImportService
{
    private readonly ITsetenContext _context;
    private readonly IEmbeddingService _embeddingService;
    private readonly ILogger<EmbeddingImportService> _logger;

    public EmbeddingImportService(
        ITsetenContext context,
        IEmbeddingService embeddingService,
        ILogger<EmbeddingImportService> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(embeddingService);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _embeddingService = embeddingService;
        _logger = logger;
    }

    public async Task ImportSoftwareRequirementAsync(SoftwareRequirement requirement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requirement);

        _logger.LogInformation("Importing software requirement {RequirementId}", requirement.SoftwareRequirementId);

        // Check if embedding already exists
        var existing = await _context.SoftwareRequirementEmbeddings
            .FirstOrDefaultAsync(e => e.SoftwareRequirementId == requirement.SoftwareRequirementId, cancellationToken);

        // Generate text for embedding (combine description with acceptance criteria)
        var textForEmbedding = BuildTextForEmbedding(requirement);

        // Generate embedding
        var embedding = await _embeddingService.GenerateEmbeddingAsync(textForEmbedding, cancellationToken);

        if (existing != null)
        {
            existing.Description = requirement.Description;
            existing.Embedding = embedding;
            existing.EmbeddingDimension = _embeddingService.EmbeddingDimension;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var embeddingEntity = new Core.SoftwareRequirementEmbedding
            {
                SoftwareRequirementId = requirement.SoftwareRequirementId,
                Description = requirement.Description,
                Embedding = embedding,
                EmbeddingDimension = _embeddingService.EmbeddingDimension,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SoftwareRequirementEmbeddings.Add(embeddingEntity);
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully imported software requirement {RequirementId}", requirement.SoftwareRequirementId);
    }

    public async Task ImportSoftwareRequirementsAsync(IEnumerable<SoftwareRequirement> requirements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requirements);

        var requirementList = requirements.ToList();

        _logger.LogInformation("Importing {Count} software requirements", requirementList.Count);

        foreach (var requirement in requirementList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ImportSoftwareRequirementAsync(requirement, cancellationToken);
        }

        _logger.LogInformation("Successfully imported {Count} software requirements", requirementList.Count);
    }

    public async Task UpdateSoftwareRequirementAsync(SoftwareRequirement requirement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requirement);

        await ImportSoftwareRequirementAsync(requirement, cancellationToken);
    }

    public async Task RemoveSoftwareRequirementAsync(string softwareRequirementId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(softwareRequirementId);

        _logger.LogInformation("Removing software requirement {RequirementId} from vector database", softwareRequirementId);

        var existing = await _context.SoftwareRequirementEmbeddings
            .FirstOrDefaultAsync(e => e.SoftwareRequirementId == softwareRequirementId, cancellationToken);

        if (existing != null)
        {
            _context.SoftwareRequirementEmbeddings.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully removed software requirement {RequirementId}", softwareRequirementId);
        }
        else
        {
            _logger.LogWarning("Software requirement {RequirementId} not found in vector database", softwareRequirementId);
        }
    }

    public async Task SyncAllSoftwareRequirementsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting full sync of software requirements to vector database");

        var allRequirements = await _context.SoftwareRequirements
            .Include(sr => sr.Comments)
            .Include(sr => sr.AcceptanceCriteria)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} software requirements to sync", allRequirements.Count);

        await ImportSoftwareRequirementsAsync(allRequirements, cancellationToken);

        _logger.LogInformation("Completed full sync of software requirements");
    }

    private static string BuildTextForEmbedding(SoftwareRequirement requirement)
    {
        var parts = new List<string> { requirement.Description };

        // Include acceptance criteria in the embedding text for richer semantic search
        if (requirement.AcceptanceCriteria?.Any() == true)
        {
            foreach (var ac in requirement.AcceptanceCriteria)
            {
                parts.Add($"Given {ac.Given} When {ac.When} Then {ac.Then}");
            }
        }

        // Include comments for additional context
        if (requirement.Comments?.Any() == true)
        {
            foreach (var comment in requirement.Comments)
            {
                parts.Add(comment.Body);
            }
        }

        return string.Join(" ", parts);
    }
}
