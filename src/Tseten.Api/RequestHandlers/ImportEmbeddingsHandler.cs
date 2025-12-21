// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;
using Tseten.Api.Services;

namespace Tseten.Api.RequestHandlers;

/// <summary>
/// Handler for importing/syncing software requirements into the vector database.
/// </summary>
public class ImportEmbeddingsHandler : IRequestHandler<ImportEmbeddingsRequest, ImportEmbeddingsResponse>
{
    private readonly ILogger<ImportEmbeddingsHandler> _logger;
    private readonly IEmbeddingImportService _embeddingImportService;
    private readonly ITsetenContext _context;

    public ImportEmbeddingsHandler(
        ILogger<ImportEmbeddingsHandler> logger,
        IEmbeddingImportService embeddingImportService,
        ITsetenContext context)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(embeddingImportService);
        ArgumentNullException.ThrowIfNull(context);

        _logger = logger;
        _embeddingImportService = embeddingImportService;
        _context = context;
    }

    public async Task<ImportEmbeddingsResponse> Handle(ImportEmbeddingsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting embedding import for all software requirements");

        try
        {
            var requirements = await _context.SoftwareRequirements
                .Include(sr => sr.Comments)
                .Include(sr => sr.AcceptanceCriteria)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var count = requirements.Count;

            await _embeddingImportService.ImportSoftwareRequirementsAsync(requirements, cancellationToken);

            _logger.LogInformation("Successfully imported {Count} software requirements", count);

            return new ImportEmbeddingsResponse
            {
                Success = true,
                ImportedCount = count,
                CompletedAt = DateTime.UtcNow,
                Message = $"Successfully imported {count} software requirements into the vector database"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing embeddings");

            return new ImportEmbeddingsResponse
            {
                Success = false,
                CompletedAt = DateTime.UtcNow,
                Errors = [$"Import failed: {ex.Message}"]
            };
        }
    }
}
