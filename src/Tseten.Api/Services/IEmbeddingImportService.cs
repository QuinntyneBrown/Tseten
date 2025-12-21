// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Services;

/// <summary>
/// Interface for importing software requirements into the vector database.
/// </summary>
public interface IEmbeddingImportService
{
    /// <summary>
    /// Imports a single software requirement into the vector database.
    /// </summary>
    Task ImportSoftwareRequirementAsync(SoftwareRequirement requirement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Imports multiple software requirements into the vector database.
    /// </summary>
    Task ImportSoftwareRequirementsAsync(IEnumerable<SoftwareRequirement> requirements, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing software requirement embedding.
    /// </summary>
    Task UpdateSoftwareRequirementAsync(SoftwareRequirement requirement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a software requirement from the vector database.
    /// </summary>
    Task RemoveSoftwareRequirementAsync(string softwareRequirementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronizes all software requirements from the main database to the vector database.
    /// </summary>
    Task SyncAllSoftwareRequirementsAsync(CancellationToken cancellationToken = default);
}
