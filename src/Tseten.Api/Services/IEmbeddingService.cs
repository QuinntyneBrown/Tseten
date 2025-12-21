// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Tseten.Api.Services;

/// <summary>
/// Interface for generating and managing text embeddings for semantic search.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>
    /// Generates an embedding vector for the given text.
    /// </summary>
    /// <param name="text">The text to generate an embedding for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A float array representing the embedding vector.</returns>
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates embeddings for multiple texts in batch.
    /// </summary>
    /// <param name="texts">The texts to generate embeddings for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of float arrays representing the embedding vectors.</returns>
    Task<List<float[]>> GenerateEmbeddingsAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the cosine similarity between two embedding vectors.
    /// </summary>
    /// <param name="embedding1">First embedding vector.</param>
    /// <param name="embedding2">Second embedding vector.</param>
    /// <returns>A similarity score between -1 and 1, where 1 indicates identical vectors.</returns>
    float CalculateCosineSimilarity(float[] embedding1, float[] embedding2);

    /// <summary>
    /// Gets the dimension of the embedding vectors produced by this service.
    /// </summary>
    int EmbeddingDimension { get; }
}
