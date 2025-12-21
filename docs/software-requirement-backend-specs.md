# Software Requirement Backend Specifications

## Overview

The Software Requirement feature provides comprehensive CRUD (Create, Read, Update, Delete) operations for managing software requirements in the Tseten application. It supports hierarchical requirements through parent-child relationships and includes a nested commenting system for collaboration.

## Architecture

| Component | Technology | Purpose |
|-----------|------------|---------|
| API Layer | ASP.NET Core 8.0 | REST API endpoints |
| CQRS Pattern | MediatR 12.4.1 | Command/Query separation |
| Data Storage | Couchbase.Lite 3.2.1 | Embedded NoSQL database |
| Validation | FluentValidation 11.10.0 | Request validation |

---

## Domain Model

### SoftwareRequirement Entity

```csharp
namespace Tseten.Models.SoftwareRequirement;

public class SoftwareRequirement
{
    public string SoftwareRequirementId { get; set; }
    public string ParentSoftwareRequirementId { get; set; }
    public string Description { get; set; }
    public bool CanImplement { get; set; }
    public bool CanTest { get; set; }
    public List<Comment> Comments { get; set; } = [];
}
```

### Comment Entity (Nested Structure)

```csharp
namespace Tseten.Models.SoftwareRequirement;

public class Comment
{
    public Guid CommentId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public string Body { get; set; }
    public string Author { get; set; }
    public bool Resolved { get; set; }
    public Comment? ParentComment { get; set; }
    public List<Comment> Comments { get; set; }
}
```

### Entity Properties

| Property | Type | Description |
|----------|------|-------------|
| SoftwareRequirementId | string | Unique identifier (primary key) |
| ParentSoftwareRequirementId | string | Reference to parent requirement for hierarchy |
| Description | string | Detailed description of the requirement |
| CanImplement | bool | Flag indicating if requirement is implementable |
| CanTest | bool | Flag indicating if requirement is testable |
| Comments | List of Comment | Nested comments for collaboration |

---

## API Specifications

### Base URL
```
/api/softwarerequirements
```

### Endpoints Summary

| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| POST | `/api/softwarerequirements` | Create a new requirement | Implemented |
| GET | `/api/softwarerequirements` | Get all requirements | Implemented |
| GET | `/api/softwarerequirements/{id}` | Get requirement by ID | Partially Implemented |
| PUT | `/api/softwarerequirements` | Update a requirement | Partially Implemented |
| DELETE | `/api/softwarerequirements/{id}` | Delete a requirement | Partially Implemented |

---

## SPEC-SR-001: Create Software Requirement

### Description
Creates a new software requirement in the system with support for hierarchical relationships and comments.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-001.1 | System SHALL accept a valid create request with description | Implemented |
| AC-001.2 | System SHALL validate that Description is not null or empty | Implemented |
| AC-001.3 | System SHALL persist the requirement to Couchbase Lite database | Implemented |
| AC-001.4 | System SHALL return the created requirement as DTO in response | Implemented |
| AC-001.5 | System SHALL return 200 OK on successful creation | Implemented |
| AC-001.6 | System SHALL return validation errors when request is invalid | Implemented |

### Request Schema

```csharp
public class CreateSoftwareRequirementRequest : IRequest<CreateSoftwareRequirementResponse>
{
    public string SoftwareRequirementId { get; set; }
    public string ParentSoftwareRequirementId { get; set; }
    public string Description { get; set; }
    public bool CanImplement { get; set; }
    public bool CanTest { get; set; }
    public List<Comment> Comments { get; set; }
}
```

### Validation Rules

```csharp
public class CreateSoftwareRequirementRequestValidator : AbstractValidator<CreateSoftwareRequirementRequest>
{
    public CreateSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| Description | NotNull | Description must not be null |
| Description | NotEmpty | Description must not be empty |

### Sample Request

```http
POST /api/softwarerequirements HTTP/1.1
Content-Type: application/json

{
    "softwareRequirementId": "REQ-001",
    "parentSoftwareRequirementId": null,
    "description": "User shall be able to login with email and password",
    "canImplement": true,
    "canTest": true,
    "comments": []
}
```

### Sample Response (Success)

```json
{
    "softwareRequirement": {
        "softwareRequirementId": "REQ-001",
        "parentSoftwareRequirementId": null,
        "description": "User shall be able to login with email and password",
        "canImplement": true,
        "canTest": true,
        "comments": []
    },
    "errors": []
}
```

### Sample Response (Validation Error)

```json
{
    "softwareRequirement": null,
    "errors": [
        "'Description' must not be empty."
    ]
}
```

### Handler Implementation

```csharp
public class CreateSoftwareRequirementHandler : IRequestHandler<CreateSoftwareRequirementRequest, CreateSoftwareRequirementResponse>
{
    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public async Task<CreateSoftwareRequirementResponse> Handle(
        CreateSoftwareRequirementRequest request,
        CancellationToken cancellationToken)
    {
        var softwareRequirement = new SoftwareRequirement()
        {
            SoftwareRequirementId = request.SoftwareRequirementId,
            ParentSoftwareRequirementId = request.ParentSoftwareRequirementId,
            Description = request.Description,
            CanImplement = request.CanImplement,
            CanTest = request.CanTest,
            Comments = request.Comments
        };

        _softwareRequirementsRepository.Create(softwareRequirement);

        return new() { SoftwareRequirement = softwareRequirement.ToDto() };
    }
}
```

---

## SPEC-SR-002: Get All Software Requirements

### Description
Retrieves all software requirements from the database.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-002.1 | System SHALL return all stored software requirements | Implemented |
| AC-002.2 | System SHALL return requirements as DTOs | Implemented |
| AC-002.3 | System SHALL return empty list when no requirements exist | Implemented |
| AC-002.4 | System SHALL return 200 OK on successful retrieval | Implemented |

### Request Schema

```csharp
public class GetSoftwareRequirementsRequest : IRequest<GetSoftwareRequirementsResponse>
{
    // No parameters required
}
```

### Sample Request

```http
GET /api/softwarerequirements HTTP/1.1
```

### Sample Response

```json
{
    "softwareRequirements": [
        {
            "softwareRequirementId": "REQ-001",
            "parentSoftwareRequirementId": null,
            "description": "User shall be able to login",
            "canImplement": true,
            "canTest": true,
            "comments": []
        },
        {
            "softwareRequirementId": "REQ-002",
            "parentSoftwareRequirementId": "REQ-001",
            "description": "Login shall support email authentication",
            "canImplement": true,
            "canTest": true,
            "comments": []
        }
    ],
    "errors": []
}
```

### Handler Implementation

```csharp
public class GetSoftwareRequirementsHandler : IRequestHandler<GetSoftwareRequirementsRequest, GetSoftwareRequirementsResponse>
{
    private readonly ISoftwareRequirementsRepository _softwareRequirementsRepository;

    public async Task<GetSoftwareRequirementsResponse> Handle(
        GetSoftwareRequirementsRequest request,
        CancellationToken cancellationToken)
    {
        var softwareRequirements = _softwareRequirementsRepository.Get();

        return new GetSoftwareRequirementsResponse()
        {
            SoftwareRequirements = softwareRequirements
                .Select(x => x.ToDto())
                .ToList()
        };
    }
}
```

---

## SPEC-SR-003: Get Software Requirement By ID

### Description
Retrieves a single software requirement by its unique identifier.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-003.1 | System SHALL return the requirement matching the ID | Not Implemented |
| AC-003.2 | System SHALL return 404 Not Found when requirement does not exist | Implemented |
| AC-003.3 | System SHALL return 200 OK on successful retrieval | Not Implemented |

### Request Schema

```csharp
public class GetSoftwareRequirementByIdRequest : IRequest<GetSoftwareRequirementByIdResponse>
{
    public string SoftwareRequirementId { get; set; }
}
```

### Sample Request

```http
GET /api/softwarerequirements/REQ-001 HTTP/1.1
```

### Sample Response (Success)

```json
{
    "softwareRequirement": {
        "softwareRequirementId": "REQ-001",
        "parentSoftwareRequirementId": null,
        "description": "User shall be able to login",
        "canImplement": true,
        "canTest": true,
        "comments": []
    },
    "errors": []
}
```

### Sample Response (Not Found)

```http
HTTP/1.1 404 Not Found

REQ-001
```

### Controller Implementation

```csharp
[HttpGet("{softwareRequirementId}", Name = "GetById")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> GetByIdAsync([FromRoute] GetSoftwareRequirementByIdRequest request)
{
    var response = await _mediator.Send(request);

    if (response.SoftwareRequirement == null)
    {
        return NotFound(request.SoftwareRequirementId);
    }

    return Ok(response);
}
```

---

## SPEC-SR-004: Update Software Requirement

### Description
Updates an existing software requirement with new values.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-004.1 | System SHALL validate SoftwareRequirementId is not null or empty | Implemented |
| AC-004.2 | System SHALL validate ParentSoftwareRequirementId is not null or empty | Implemented |
| AC-004.3 | System SHALL validate Description is not null or empty | Implemented |
| AC-004.4 | System SHALL persist updated requirement to database | Not Implemented |
| AC-004.5 | System SHALL return updated requirement as DTO | Not Implemented |

### Request Schema

```csharp
public class UpdateSoftwareRequirementRequest : IRequest<UpdateSoftwareRequirementResponse>
{
    public string SoftwareRequirementId { get; set; }
    public string ParentSoftwareRequirementId { get; set; }
    public string Description { get; set; }
    public bool CanImplement { get; set; }
    public bool CanTest { get; set; }
    public List<Comment> Comments { get; set; }
}
```

### Validation Rules

```csharp
public class UpdateSoftwareRequirementRequestValidator : AbstractValidator<UpdateSoftwareRequirementRequest>
{
    public UpdateSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.SoftwareRequirementId).NotNull().NotEmpty();
        RuleFor(x => x.ParentSoftwareRequirementId).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
    }
}
```

| Field | Rule | Error Message |
|-------|------|---------------|
| SoftwareRequirementId | NotNull, NotEmpty | Must be provided |
| ParentSoftwareRequirementId | NotNull, NotEmpty | Must be provided |
| Description | NotNull, NotEmpty | Must be provided |

### Sample Request

```http
PUT /api/softwarerequirements HTTP/1.1
Content-Type: application/json

{
    "softwareRequirementId": "REQ-001",
    "parentSoftwareRequirementId": "REQ-000",
    "description": "User shall be able to login with email or username",
    "canImplement": true,
    "canTest": true,
    "comments": []
}
```

---

## SPEC-SR-005: Delete Software Requirement

### Description
Deletes a software requirement from the system.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-005.1 | System SHALL validate SoftwareRequirementId is not null or empty | Implemented |
| AC-005.2 | System SHALL remove requirement from database | Not Implemented |
| AC-005.3 | System SHALL return success response on deletion | Not Implemented |

### Request Schema

```csharp
public class DeleteSoftwareRequirementRequest : IRequest<DeleteSoftwareRequirementResponse>
{
    public string SoftwareRequirementId { get; set; }
}
```

### Validation Rules

```csharp
public class DeleteSoftwareRequirementRequestValidator : AbstractValidator<DeleteSoftwareRequirementRequest>
{
    public DeleteSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.SoftwareRequirementId).NotNull().NotEmpty();
    }
}
```

### Sample Request

```http
DELETE /api/softwarerequirements/REQ-001 HTTP/1.1
```

---

## Repository Implementation

### Interface Definition

```csharp
public interface ISoftwareRequirementsRepository
{
    void Create(SoftwareRequirement softwareRequirement);
    void Update(SoftwareRequirement softwareRequirement);
    void Delete(string softwareRequirementId);
    SoftwareRequirement GetById(string softwareRequirementId);
    List<SoftwareRequirement> Get();
}
```

### Repository Implementation Status

| Method | Status | Description |
|--------|--------|-------------|
| Create | Implemented | Persists document to Couchbase Lite |
| Get | Implemented | Retrieves all documents using QueryBuilder |
| GetById | Not Implemented | Throws NotImplementedException |
| Update | Not Implemented | Throws NotImplementedException |
| Delete | Not Implemented | Throws NotImplementedException |

### Create Implementation

```csharp
public void Create(SoftwareRequirement softwareRequirement)
{
    var mutableDocument = new MutableDocument(
        softwareRequirement.SoftwareRequirementId,
        JsonSerializer.Serialize(softwareRequirement, _options));

    _database.GetDefaultCollection().Save(mutableDocument);
}
```

### Get Implementation

```csharp
public List<SoftwareRequirement> Get()
{
    var result = new List<SoftwareRequirement>();

    var query = QueryBuilder.Select(SelectResult.All())
        .From(DataSource.Collection(_database.GetDefaultCollection()));

    foreach (var item in query.Execute())
    {
        var json = item.ToJSON();
        var softwareRequirement = JsonSerializer.Deserialize<Dictionary<string, SoftwareRequirement>>(json);
        result.Add(softwareRequirement!.Single().Value);
    }

    return result;
}
```

---

## Data Storage

### Database Configuration

| Setting | Value |
|---------|-------|
| Database Type | Couchbase Lite (Embedded NoSQL) |
| Database Name | software-requirements |
| Registration | Singleton lifetime |

### Serialization Options

```csharp
private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true,
    ReferenceHandler = ReferenceHandler.IgnoreCycles
};
```

---

## Dependency Injection

```csharp
public static void AddApiServices(this IServiceCollection services,
    Action<CorsPolicyBuilder> configureCorsPolicyBuilder,
    string connectionString)
{
    services.AddSingleton<ISoftwareRequirementsRepository, SoftwareRequirementsRepository>();
    services.AddControllers();

    var db = new Database("software-requirements");
    services.AddSingleton(db);

    services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());
}
```

---

## Error Handling

### Response Base Structure

All responses inherit from `ResponseBase` which includes an errors collection:

```csharp
public class ResponseBase
{
    public ResponseBase()
    {
        Errors = new List<string>();
    }
    public List<string> Errors { get; set; }
}
```

### Validation Error Flow

1. Request is received by controller
2. MediatR dispatches to ValidationBehavior pipeline
3. FluentValidation validators are executed
4. If validation fails, errors are collected and returned without calling handler
5. If validation passes, request proceeds to handler

---

## Technical Notes

- All handlers use constructor injection for dependencies
- Null argument validation using `ArgumentNullException.ThrowIfNull()`
- Async/await pattern used throughout
- DTO mapping via extension method `ToDto()`
