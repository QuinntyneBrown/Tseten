# Vector Search Backend Specifications

## Overview

The Vector Search feature provides semantic search capabilities for software requirements using vector embeddings stored in SQL Server/SQL Express. This enables users to search requirements by meaning rather than exact keyword matches, improving discoverability and requirement analysis.

## Architecture

| Component | Technology | Purpose |
|-----------|------------|---------|
| Vector Storage | SQL Server/SQL Express | Stores embeddings with EF Core |
| Embedding Service | Custom .NET Service | Generates text embeddings |
| Similarity Search | Cosine Similarity | Ranks results by semantic relevance |
| API Layer | ASP.NET Core 8.0 | REST API endpoints |
| CQRS Pattern | MediatR 12.4.1 | Command/Query separation |

---

## Domain Model

### SoftwareRequirementEmbedding Entity

```csharp
namespace Tseten.Models.SoftwareRequirement;

public class SoftwareRequirementEmbedding
{
    public int Id { get; set; }                        // Primary key
    public string SoftwareRequirementId { get; set; }  // Foreign key reference
    public string Description { get; set; }            // Cached description text
    public string EmbeddingVector { get; set; }        // Comma-separated float values
    public int EmbeddingDimension { get; set; }        // Vector dimension (default: 384)
    public DateTime CreatedAt { get; set; }            // Creation timestamp
    public DateTime UpdatedAt { get; set; }            // Last update timestamp

    // Helper property for easy access
    public float[] Embedding { get; set; }             // Converted float array
}
```

### Entity Properties

| Property | Type | Description |
|----------|------|-------------|
| Id | int | Auto-generated primary key |
| SoftwareRequirementId | string | Reference to the original requirement |
| Description | string | Cached description for quick access |
| EmbeddingVector | nvarchar(max) | Serialized embedding vector |
| EmbeddingDimension | int | Dimension of the embedding (384 for MiniLM) |
| CreatedAt | DateTime | When the embedding was created |
| UpdatedAt | DateTime | When the embedding was last updated |

---

## API Specifications

### Base URL
```
/api/softwarerequirements
```

### Endpoints Summary

| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| GET | `/api/softwarerequirements/search` | Semantic search with pagination | Implemented |
| POST | `/api/softwarerequirements/embeddings/import` | Import/sync all embeddings | Implemented |

---

## SPEC-VS-001: Semantic Search

### Description
Performs paginated semantic search of software requirements using vector similarity. Returns results ranked by relevance with similarity scores.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-001.1 | System SHALL accept search query with pagination parameters | Implemented |
| AC-001.2 | System SHALL validate query is not empty and within length limits | Implemented |
| AC-001.3 | System SHALL generate embedding for search query | Implemented |
| AC-001.4 | System SHALL calculate cosine similarity with all stored embeddings | Implemented |
| AC-001.5 | System SHALL filter results by minimum similarity threshold | Implemented |
| AC-001.6 | System SHALL return paginated results with similarity scores | Implemented |
| AC-001.7 | System SHALL support optional CanImplement/CanTest filters | Implemented |
| AC-001.8 | System SHALL return highlighted text snippets | Implemented |

### Request Schema

```csharp
public class SearchSoftwareRequirementsRequest : IRequest<SearchSoftwareRequirementsResponse>
{
    public string Query { get; set; }           // Search query text
    public int Page { get; set; } = 1;          // Page number (1-based)
    public int PageSize { get; set; } = 10;     // Items per page
    public float MinSimilarity { get; set; }    // Minimum similarity threshold (0-1)
    public bool? CanImplement { get; set; }     // Optional filter
    public bool? CanTest { get; set; }          // Optional filter
}
```

### Validation Rules

```csharp
public class SearchSoftwareRequirementsRequestValidator : AbstractValidator<SearchSoftwareRequirementsRequest>
{
    public SearchSoftwareRequirementsRequestValidator()
    {
        RuleFor(x => x.Query).NotNull().NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.MinSimilarity).InclusiveBetween(0f, 1f);
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| Query | NotNull, NotEmpty | Search query is required |
| Query | MaximumLength(1000) | Query must not exceed 1000 characters |
| Page | GreaterThanOrEqualTo(1) | Page must be at least 1 |
| PageSize | InclusiveBetween(1, 100) | Page size must be between 1 and 100 |
| MinSimilarity | InclusiveBetween(0, 1) | Similarity must be between 0 and 1 |

### Sample Request

```http
GET /api/softwarerequirements/search?query=user+authentication&page=1&pageSize=10&minSimilarity=0.5 HTTP/1.1
```

### Sample Response (Success)

```json
{
    "results": [
        {
            "softwareRequirement": {
                "softwareRequirementId": "REQ-001",
                "description": "User shall be able to login with email and password",
                "canImplement": true,
                "canTest": true
            },
            "similarityScore": 0.85,
            "highlight": "...User shall be able to login..."
        },
        {
            "softwareRequirement": {
                "softwareRequirementId": "REQ-002",
                "description": "System shall support OAuth2 authentication",
                "canImplement": true,
                "canTest": true
            },
            "similarityScore": 0.72,
            "highlight": "...OAuth2 authentication..."
        }
    ],
    "page": 1,
    "pageSize": 10,
    "totalCount": 25,
    "totalPages": 3,
    "hasNextPage": true,
    "hasPreviousPage": false,
    "query": "user authentication",
    "errors": []
}
```

### Sample Response (Validation Error)

```json
{
    "results": [],
    "page": 1,
    "pageSize": 10,
    "totalCount": 0,
    "totalPages": 0,
    "hasNextPage": false,
    "hasPreviousPage": false,
    "query": "",
    "errors": [
        "'Query' must not be empty."
    ]
}
```

---

## SPEC-VS-002: Import Embeddings

### Description
Imports/syncs all software requirements into the vector database by generating embeddings for each requirement.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-002.1 | System SHALL retrieve all software requirements | Implemented |
| AC-002.2 | System SHALL generate embeddings for each requirement | Implemented |
| AC-002.3 | System SHALL persist embeddings to SQL database | Implemented |
| AC-002.4 | System SHALL update existing embeddings on re-import | Implemented |
| AC-002.5 | System SHALL return count of imported requirements | Implemented |

### Request Schema

```csharp
public class ImportEmbeddingsRequest : IRequest<ImportEmbeddingsResponse>
{
    // No parameters required
}
```

### Sample Request

```http
POST /api/softwarerequirements/embeddings/import HTTP/1.1
```

### Sample Response (Success)

```json
{
    "success": true,
    "importedCount": 150,
    "completedAt": "2024-01-15T10:30:00Z",
    "message": "Successfully imported 150 software requirements into the vector database",
    "errors": []
}
```

---

## Vector Database Configuration

### SQL Server Connection

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TsetenVectorDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    }
}
```

### Entity Framework Configuration

```csharp
services.AddDbContext<VectorDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));
```

### Database Schema

```sql
CREATE TABLE [dbo].[SoftwareRequirementEmbeddings] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [SoftwareRequirementId] NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(2000) NOT NULL,
    [EmbeddingVector] NVARCHAR(MAX) NOT NULL,
    [EmbeddingDimension] INT NOT NULL DEFAULT 384,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE UNIQUE INDEX [IX_SoftwareRequirementEmbeddings_SoftwareRequirementId]
ON [dbo].[SoftwareRequirementEmbeddings] ([SoftwareRequirementId]);
```

---

## Embedding Service

### Interface

```csharp
public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    Task<List<float[]>> GenerateEmbeddingsAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default);
    float CalculateCosineSimilarity(float[] embedding1, float[] embedding2);
    int EmbeddingDimension { get; }
}
```

### Cosine Similarity Algorithm

```csharp
public float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
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
```

### SIMD Acceleration

The embedding service uses SIMD (Single Instruction, Multiple Data) acceleration when available for faster similarity calculations:

```csharp
if (Vector.IsHardwareAccelerated && embedding1.Length >= Vector<float>.Count)
{
    return CalculateCosineSimilaritySimd(embedding1, embedding2);
}
```

---

## Dependency Injection

```csharp
public static void AddApiServices(this IServiceCollection services, ...)
{
    // Vector database context
    services.AddDbContext<VectorDbContext>(options =>
        options.UseSqlServer(connectionString, ...));

    // Embedding services
    services.AddSingleton<IEmbeddingService, EmbeddingService>();
    services.AddScoped<IEmbeddingImportService, EmbeddingImportService>();
}
```

---

## Best Practices and Resources

### Vector Database Best Practices

1. **Embedding Dimension Selection**
   - 384 dimensions: Good for MiniLM models (fast, efficient)
   - 768 dimensions: Standard for BERT-based models
   - 1536 dimensions: OpenAI ada-002 embeddings (highest quality)

2. **Indexing Strategies**
   - For small datasets (<10K): In-memory comparison is sufficient
   - For medium datasets (10K-100K): Consider approximate nearest neighbor (ANN)
   - For large datasets (>100K): Use specialized vector databases

3. **Query Optimization**
   - Set appropriate MinSimilarity thresholds (0.5-0.7 typical)
   - Use pagination to limit result sets
   - Consider caching frequent queries

### SQL Server Vector Search

**Official Documentation:**
- [SQL Server 2022 Vector Support](https://learn.microsoft.com/en-us/sql/relational-databases/vectors/vectors-sql-server)
- [Azure SQL Vector Search Preview](https://learn.microsoft.com/en-us/azure/azure-sql/database/vector-search)

**Community Resources:**
- [Building RAG Applications with SQL Server](https://devblogs.microsoft.com/azure-sql/rag-with-azure-sql-database/)
- [Vector Similarity Search in SQL Server](https://www.sqlshack.com/implementing-vector-similarity-search-in-sql-server/)

### .NET Embedding Libraries

**Production-Ready Options:**

1. **Azure OpenAI / OpenAI API**
   - [Azure.AI.OpenAI NuGet](https://www.nuget.org/packages/Azure.AI.OpenAI)
   - [OpenAI .NET Client](https://www.nuget.org/packages/OpenAI)
   - Best quality embeddings (ada-002, text-embedding-3-small/large)

2. **Microsoft Semantic Kernel**
   - [Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
   - Provides embedding generation and memory abstractions
   - Supports multiple embedding providers

3. **ML.NET with ONNX Runtime**
   - [ML.NET Documentation](https://learn.microsoft.com/en-us/dotnet/machine-learning/)
   - [ONNX Runtime](https://onnxruntime.ai/)
   - Run local embedding models (MiniLM, BERT)

4. **Hugging Face Inference API**
   - [Hugging Face .NET Client](https://github.com/huggingface/huggingface_hub)
   - Access to thousands of embedding models

### Vector Database Alternatives

**Specialized Vector Databases:**
- [Pinecone](https://www.pinecone.io/) - Managed vector database
- [Qdrant](https://qdrant.tech/) - Open-source, Rust-based
- [Milvus](https://milvus.io/) - Open-source, highly scalable
- [Weaviate](https://weaviate.io/) - Open-source with GraphQL API
- [Chroma](https://www.trychroma.com/) - Simple, developer-friendly

**Integrated Solutions:**
- [PostgreSQL pgvector](https://github.com/pgvector/pgvector) - PostgreSQL extension
- [Redis Vector Similarity](https://redis.io/docs/stack/search/reference/vectors/) - Redis Stack
- [Elasticsearch Vector Search](https://www.elastic.co/guide/en/elasticsearch/reference/current/dense-vector.html)

### Recommended Reading

**Vector Search Fundamentals:**
- [Understanding Vector Embeddings](https://www.pinecone.io/learn/vector-embeddings/)
- [Cosine Similarity Explained](https://www.learndatasci.com/glossary/cosine-similarity/)
- [Approximate Nearest Neighbors](https://www.pinecone.io/learn/what-is-similarity-search/)

**RAG (Retrieval-Augmented Generation):**
- [RAG Pattern Overview](https://learn.microsoft.com/en-us/azure/architecture/ai-ml/architecture/rag-pattern)
- [Building RAG Applications](https://www.deeplearning.ai/short-courses/building-applications-with-vector-databases/)

**Performance Optimization:**
- [Vector Search at Scale](https://www.pinecone.io/learn/vector-search-optimization/)
- [HNSW Algorithm](https://www.pinecone.io/learn/hnsw/) - Hierarchical Navigable Small World graphs

### .NET-Specific Articles

- [Semantic Memory in .NET](https://devblogs.microsoft.com/semantic-kernel/announcing-semantic-kernel-memory/)
- [Vector Search with Azure Cognitive Search](https://learn.microsoft.com/en-us/azure/search/vector-search-overview)
- [Building Intelligent Apps with .NET](https://devblogs.microsoft.com/dotnet/build-intelligent-apps-with-dotnet/)

---

## Technical Notes

1. **Embedding Generation**: The current implementation uses a hash-based approach for demonstration. For production, integrate with OpenAI, Azure OpenAI, or local ONNX models.

2. **Vector Storage**: Embeddings are stored as comma-separated strings in SQL Express for compatibility. SQL Server 2022+ supports native vector types.

3. **Performance**: SIMD acceleration is used for similarity calculations. For large datasets, consider batch processing and async operations.

4. **Scaling**: The in-memory similarity search is suitable for <10K requirements. For larger datasets, consider specialized vector databases or SQL Server's native vector features.

5. **Text Preprocessing**: Descriptions are normalized (lowercase, whitespace collapsed) before embedding generation.

6. **Embedding Updates**: When requirements change, call the import endpoint to regenerate embeddings.

---

## Error Handling

### Response Structure

All responses include an errors collection:

```csharp
public class SearchSoftwareRequirementsResponse
{
    public List<SearchResultDto> Results { get; set; }
    public int TotalCount { get; set; }
    // ... pagination properties
    public List<string> Errors { get; set; } = [];
}
```

### Error Scenarios

| Scenario | HTTP Status | Error Message |
|----------|-------------|---------------|
| Empty query | 400 | Search query is required |
| Query too long | 400 | Query must not exceed 1000 characters |
| Invalid page | 400 | Page must be at least 1 |
| Database error | 400 | Search failed: [error details] |
| No embeddings found | 200 | Empty results (not an error) |
