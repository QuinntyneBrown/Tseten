// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Tseten.Api.Services;

/// <summary>
/// Embedding service that generates vector embeddings for text.
/// Uses a deterministic hash-based approach for demonstration purposes.
/// In production, replace with Azure OpenAI, OpenAI API, or local model (e.g., ONNX Runtime with MiniLM).
/// </summary>
public class EmbeddingService : IEmbeddingService
{
    private readonly ILogger<EmbeddingService> _logger;
    private const int DefaultEmbeddingDimension = 384;

    public EmbeddingService(ILogger<EmbeddingService> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public int EmbeddingDimension => DefaultEmbeddingDimension;

    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            _logger.LogWarning("Empty text provided for embedding generation");
            return Task.FromResult(new float[EmbeddingDimension]);
        }

        // Normalize text
        var normalizedText = NormalizeText(text);

        // Generate embedding using hash-based approach
        var embedding = GenerateHashBasedEmbedding(normalizedText);

        _logger.LogDebug("Generated embedding for text of length {TextLength}", text.Length);

        return Task.FromResult(embedding);
    }

    public async Task<List<float[]>> GenerateEmbeddingsAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var embeddings = new List<float[]>();

        foreach (var text in texts)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var embedding = await GenerateEmbeddingAsync(text, cancellationToken);
            embeddings.Add(embedding);
        }

        _logger.LogInformation("Generated {Count} embeddings", embeddings.Count);

        return embeddings;
    }

    public float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
    {
        if (embedding1.Length != embedding2.Length)
        {
            throw new ArgumentException("Embeddings must have the same dimension");
        }

        if (embedding1.Length == 0)
        {
            return 0f;
        }

        // Use SIMD-accelerated operations when available
        if (Vector.IsHardwareAccelerated && embedding1.Length >= Vector<float>.Count)
        {
            return CalculateCosineSimilaritySimd(embedding1, embedding2);
        }

        return CalculateCosineSimilarityScalar(embedding1, embedding2);
    }

    private static string NormalizeText(string text)
    {
        return text.ToLowerInvariant()
            .Trim()
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\t", " ");
    }

    private float[] GenerateHashBasedEmbedding(string text)
    {
        var embedding = new float[EmbeddingDimension];

        // Create deterministic embedding using SHA256 hash expanded to desired dimension
        var textBytes = Encoding.UTF8.GetBytes(text);

        // Generate multiple hash iterations to fill the embedding dimension
        var hashIterations = (EmbeddingDimension * sizeof(float)) / 32 + 1;
        var allBytes = new byte[hashIterations * 32];

        for (int i = 0; i < hashIterations; i++)
        {
            var iterationBytes = Encoding.UTF8.GetBytes($"{text}_{i}");
            var hash = SHA256.HashData(iterationBytes);
            Buffer.BlockCopy(hash, 0, allBytes, i * 32, 32);
        }

        // Convert bytes to floats and normalize
        for (int i = 0; i < EmbeddingDimension; i++)
        {
            int byteIndex = i * 4;
            if (byteIndex + 3 < allBytes.Length)
            {
                embedding[i] = BitConverter.ToSingle(allBytes, byteIndex);
            }
        }

        // Normalize the embedding vector
        NormalizeVector(embedding);

        // Additionally incorporate word-level features for better semantic similarity
        IncorporateWordFeatures(embedding, text);
        NormalizeVector(embedding);

        return embedding;
    }

    private static void IncorporateWordFeatures(float[] embedding, string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length && i < embedding.Length / 2; i++)
        {
            var wordHash = words[i].GetHashCode();
            var index = Math.Abs(wordHash) % embedding.Length;
            embedding[index] += (float)(Math.Sin(wordHash) * 0.1);

            // Add bigram features
            if (i > 0)
            {
                var bigramHash = (words[i - 1] + " " + words[i]).GetHashCode();
                var bigramIndex = Math.Abs(bigramHash) % embedding.Length;
                embedding[bigramIndex] += (float)(Math.Cos(bigramHash) * 0.05);
            }
        }
    }

    private static void NormalizeVector(float[] vector)
    {
        float magnitude = 0f;
        for (int i = 0; i < vector.Length; i++)
        {
            // Handle NaN or Infinity
            if (float.IsNaN(vector[i]) || float.IsInfinity(vector[i]))
            {
                vector[i] = 0f;
            }
            magnitude += vector[i] * vector[i];
        }

        magnitude = MathF.Sqrt(magnitude);

        if (magnitude > 0)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] /= magnitude;
            }
        }
    }

    private static float CalculateCosineSimilaritySimd(float[] embedding1, float[] embedding2)
    {
        var dotProduct = 0f;
        var magnitude1 = 0f;
        var magnitude2 = 0f;

        int simdLength = Vector<float>.Count;
        int i = 0;

        // SIMD-accelerated computation
        for (; i <= embedding1.Length - simdLength; i += simdLength)
        {
            var v1 = new Vector<float>(embedding1, i);
            var v2 = new Vector<float>(embedding2, i);

            dotProduct += Vector.Dot(v1, v2);
            magnitude1 += Vector.Dot(v1, v1);
            magnitude2 += Vector.Dot(v2, v2);
        }

        // Handle remaining elements
        for (; i < embedding1.Length; i++)
        {
            dotProduct += embedding1[i] * embedding2[i];
            magnitude1 += embedding1[i] * embedding1[i];
            magnitude2 += embedding2[i] * embedding2[i];
        }

        var magnitude = MathF.Sqrt(magnitude1) * MathF.Sqrt(magnitude2);

        return magnitude > 0 ? dotProduct / magnitude : 0f;
    }

    private static float CalculateCosineSimilarityScalar(float[] embedding1, float[] embedding2)
    {
        float dotProduct = 0f;
        float magnitude1 = 0f;
        float magnitude2 = 0f;

        for (int i = 0; i < embedding1.Length; i++)
        {
            dotProduct += embedding1[i] * embedding2[i];
            magnitude1 += embedding1[i] * embedding1[i];
            magnitude2 += embedding2[i] * embedding2[i];
        }

        var magnitude = MathF.Sqrt(magnitude1) * MathF.Sqrt(magnitude2);

        return magnitude > 0 ? dotProduct / magnitude : 0f;
    }
}
