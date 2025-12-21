# Tag Backend Specifications

## Overview

The Tag feature provides a tagging system for categorizing and organizing software requirements. Tags allow users to label requirements with descriptive metadata for filtering and grouping purposes.

**Implementation Status**: Models, DTOs, Requests, Responses, and Validators are defined. API Controller, Handlers, and Repository are NOT yet implemented.

## Architecture

| Component | Technology | Status |
|-----------|------------|--------|
| Domain Models | C# Classes | Implemented |
| DTOs | C# Classes | Implemented |
| Request/Response | MediatR IRequest | Implemented |
| Validators | FluentValidation | Implemented |
| API Controller | ASP.NET Core | Not Implemented |
| Request Handlers | MediatR IRequestHandler | Not Implemented |
| Repository | Couchbase.Lite | Not Implemented |

---

## Domain Model

### Tag Entity

```csharp
namespace Tag.Models.Tag;

public class Tag
{
    public Guid TagId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### Tag DTO

```csharp
namespace Tag.Models.Tag;

public class TagDto
{
    public Guid TagId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### Entity Properties

| Property | Type | Description |
|----------|------|-------------|
| TagId | Guid | Unique identifier (primary key) |
| Name | string | Short name for the tag |
| Description | string | Detailed description of tag purpose |

---

## API Specifications (Planned)

### Base URL (Planned)
```
/api/tags
```

### Endpoints Summary (Planned)

| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| POST | `/api/tags` | Create a new tag | Not Implemented |
| GET | `/api/tags` | Get all tags | Not Implemented |
| GET | `/api/tags/{id}` | Get tag by ID | Not Implemented |
| PUT | `/api/tags` | Update a tag | Not Implemented |
| DELETE | `/api/tags/{id}` | Delete a tag | Not Implemented |

---

## SPEC-TAG-001: Create Tag

### Description
Creates a new tag in the system for categorizing software requirements.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-001.1 | System SHALL accept a valid create request with name and description | Validation Ready |
| AC-001.2 | System SHALL validate that Name is not null or empty | Validation Ready |
| AC-001.3 | System SHALL validate that Description is not null or empty | Validation Ready |
| AC-001.4 | System SHALL generate a unique TagId (Guid) | Not Implemented |
| AC-001.5 | System SHALL persist the tag to database | Not Implemented |
| AC-001.6 | System SHALL return the created tag as DTO in response | Not Implemented |

### Request Schema

```csharp
namespace Tag.Models.Tag;

public class CreateTagRequest : IRequest<CreateTagResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### Response Schema

```csharp
namespace Tag.Models.Tag;

public class CreateTagResponse : ResponseBase
{
    public TagDto Tag { get; set; }
}
```

### Validation Rules

```csharp
namespace Tag.Models.Tag;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| Name | NotNull | Name must not be null |
| Name | NotEmpty | Name must not be empty |
| Description | NotNull | Description must not be null |
| Description | NotEmpty | Description must not be empty |

### Sample Request (Planned)

```http
POST /api/tags HTTP/1.1
Content-Type: application/json

{
    "name": "Security",
    "description": "Requirements related to application security"
}
```

### Sample Response (Planned)

```json
{
    "tag": {
        "tagId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Security",
        "description": "Requirements related to application security"
    },
    "errors": []
}
```

---

## SPEC-TAG-002: Get All Tags

### Description
Retrieves all tags from the database.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-002.1 | System SHALL return all stored tags | Not Implemented |
| AC-002.2 | System SHALL return tags as DTOs | Not Implemented |
| AC-002.3 | System SHALL return empty list when no tags exist | Not Implemented |

### Request Schema

```csharp
namespace Tag.Models.Tag;

public class GetTagsRequest : IRequest<GetTagsResponse>
{
    // No parameters required
}
```

### Response Schema

```csharp
namespace Tag.Models.Tag;

public class GetTagsResponse : ResponseBase
{
    public List<TagDto> Tags { get; set; }
}
```

### Sample Response (Planned)

```json
{
    "tags": [
        {
            "tagId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "Security",
            "description": "Security related requirements"
        },
        {
            "tagId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
            "name": "Performance",
            "description": "Performance related requirements"
        }
    ],
    "errors": []
}
```

---

## SPEC-TAG-003: Get Tag By ID

### Description
Retrieves a single tag by its unique identifier.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-003.1 | System SHALL return the tag matching the ID | Not Implemented |
| AC-003.2 | System SHALL return 404 Not Found when tag does not exist | Not Implemented |

### Request Schema

```csharp
namespace Tag.Models.Tag;

public class GetTagByIdRequest : IRequest<GetTagByIdResponse>
{
    public Guid TagId { get; set; }
}
```

### Response Schema

```csharp
namespace Tag.Models.Tag;

public class GetTagByIdResponse : ResponseBase
{
    public TagDto Tag { get; set; }
}
```

### Sample Request (Planned)

```http
GET /api/tags/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
```

---

## SPEC-TAG-004: Update Tag

### Description
Updates an existing tag with new values.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-004.1 | System SHALL validate TagId is not null (Guid default check) | Validation Ready |
| AC-004.2 | System SHALL validate Name is not null or empty | Validation Ready |
| AC-004.3 | System SHALL validate Description is not null or empty | Validation Ready |
| AC-004.4 | System SHALL persist updated tag to database | Not Implemented |
| AC-004.5 | System SHALL return updated tag as DTO | Not Implemented |

### Request Schema

```csharp
namespace Tag.Models.Tag;

public class UpdateTagRequest : IRequest<UpdateTagResponse>
{
    public Guid TagId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### Response Schema

```csharp
namespace Tag.Models.Tag;

public class UpdateTagResponse : ResponseBase
{
    public TagDto Tag { get; set; }
}
```

### Validation Rules

```csharp
namespace Tag.Models.Tag;

public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {
        RuleFor(x => x.TagId).NotNull();
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| TagId | NotNull | TagId must be provided |
| Name | NotNull, NotEmpty | Name must be provided |
| Description | NotNull, NotEmpty | Description must be provided |

### Sample Request (Planned)

```http
PUT /api/tags HTTP/1.1
Content-Type: application/json

{
    "tagId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Security Updated",
    "description": "Updated security requirements description"
}
```

---

## SPEC-TAG-005: Delete Tag

### Description
Deletes a tag from the system.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-005.1 | System SHALL validate TagId is not null | Validation Ready |
| AC-005.2 | System SHALL remove tag from database | Not Implemented |
| AC-005.3 | System SHALL return success response on deletion | Not Implemented |

### Request Schema

```csharp
namespace Tag.Models.Tag;

public class DeleteTagRequest : IRequest<DeleteTagResponse>
{
    public Guid TagId { get; set; }
}
```

### Response Schema

```csharp
namespace Tag.Models.Tag;

public class DeleteTagResponse : ResponseBase
{
    // Success indicated by empty errors list
}
```

### Validation Rules

```csharp
namespace Tag.Models.Tag;

public class DeleteTagRequestValidator : AbstractValidator<DeleteTagRequest>
{
    public DeleteTagRequestValidator()
    {
        RuleFor(x => x.TagId).NotNull();
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| TagId | NotNull | TagId must be provided |

### Sample Request (Planned)

```http
DELETE /api/tags/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
```

---

## Implementation Roadmap

### Completed Components

| Component | File Path |
|-----------|-----------|
| Tag Entity | `src/SpecSync.Models/Tag/Tag.cs` |
| TagDto | `src/SpecSync.Models/Tag/TagDto.cs` |
| TagExtensions | `src/SpecSync.Models/Tag/TagExtensions.cs` |
| CreateTagRequest | `src/SpecSync.Models/Tag/CreateTagRequest.cs` |
| CreateTagResponse | `src/SpecSync.Models/Tag/CreateTagResponse.cs` |
| CreateTagRequestValidator | `src/SpecSync.Models/Tag/CreateTagRequestValidator.cs` |
| GetTagsRequest | `src/SpecSync.Models/Tag/GetTagsRequest.cs` |
| GetTagsResponse | `src/SpecSync.Models/Tag/GetTagsResponse.cs` |
| GetTagByIdRequest | `src/SpecSync.Models/Tag/GetTagByIdRequest.cs` |
| GetTagByIdResponse | `src/SpecSync.Models/Tag/GetTagByIdResponse.cs` |
| UpdateTagRequest | `src/SpecSync.Models/Tag/UpdateTagRequest.cs` |
| UpdateTagResponse | `src/SpecSync.Models/Tag/UpdateTagResponse.cs` |
| UpdateTagRequestValidator | `src/SpecSync.Models/Tag/UpdateTagRequestValidator.cs` |
| DeleteTagRequest | `src/SpecSync.Models/Tag/DeleteTagRequest.cs` |
| DeleteTagResponse | `src/SpecSync.Models/Tag/DeleteTagResponse.cs` |
| DeleteTagRequestValidator | `src/SpecSync.Models/Tag/DeleteTagRequestValidator.cs` |

### Required Components (Not Implemented)

| Component | Suggested File Path |
|-----------|---------------------|
| TagsController | `src/SpecSync.SoftwareRequirements.Api/Controllers/TagsController.cs` |
| ITagsRepository | `src/SpecSync.SoftwareRequirements.Api/ITagsRepository.cs` |
| TagsRepository | `src/SpecSync.SoftwareRequirements.Api/TagsRepository.cs` |
| CreateTagHandler | `src/SpecSync.SoftwareRequirements.Api/RequestHandlers/CreateTagHandler.cs` |
| GetTagsHandler | `src/SpecSync.SoftwareRequirements.Api/RequestHandlers/GetTagsHandler.cs` |
| GetTagByIdHandler | `src/SpecSync.SoftwareRequirements.Api/RequestHandlers/GetTagByIdHandler.cs` |
| UpdateTagHandler | `src/SpecSync.SoftwareRequirements.Api/RequestHandlers/UpdateTagHandler.cs` |
| DeleteTagHandler | `src/SpecSync.SoftwareRequirements.Api/RequestHandlers/DeleteTagHandler.cs` |

---

## Technical Notes

- Tag models use `Guid` for TagId (unlike SoftwareRequirement which uses `string`)
- All validators follow the same FluentValidation pattern
- Namespace is `Tag.Models.Tag` (distinct from SoftwareRequirement namespace)
- Extension method `ToDto()` should map Tag entity to TagDto
