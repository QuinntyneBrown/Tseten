# PlantUML Documentation Guidelines for Tseten

## Overview

This document describes the specifications, conventions, and guidelines used to create PlantUML documentation for the Tseten project. These guidelines enable a coding agent to reproduce the entire solution architecture by reading the PlantUML documents.

## Document Purpose

The PlantUML documents in the `/plantuml` directory serve as:
1. **Architecture Blueprint** - Complete visual representation of the system
2. **Code Generation Source** - Detailed enough for agents to generate the full codebase
3. **Onboarding Resource** - Quick understanding of system structure for new developers
4. **Documentation Standard** - Maintainable, version-controlled architecture docs

## File Organization

### .NET Solution Documents

1. **dotnet-solution-architecture.puml** - Overall solution structure
   - Shows 4 projects: Api, Core, Infrastructure, and their test projects
   - Dependencies between projects
   - Connection to databases
   - Layer separation (Presentation, Domain, Infrastructure, Test)

2. **dotnet-core-domain-models.puml** - Domain model details
   - All aggregates: SoftwareRequirement, Tag, User, Profile, Role, InvitationToken
   - Entity properties with types
   - Relationships between entities
   - Enumerations (Priority, Status, Privilege)
   - Vector embedding model

3. **dotnet-cqrs-pattern.puml** - CQRS implementation with MediatR
   - All request/response DTOs for CRUD operations
   - Handler classes for each operation
   - ValidationBehavior pipeline
   - Validator classes
   - MediatR interface implementations

4. **dotnet-api-endpoints.puml** - REST API specification
   - All controller endpoints with routes
   - HTTP methods (GET, POST, PUT, DELETE)
   - Request/response types
   - Query parameters
   - Route parameters
   - Services used by controllers

5. **dotnet-database-architecture.puml** - Database strategy
   - Dual database approach
   - Couchbase Lite collections and schemas
   - SQL Server tables and schemas
   - Context classes (TsetenContext, VectorDbContext)
   - EmbeddingService operations

6. **dotnet-authentication-flow.puml** - Security implementation
   - Authentication sequence (login flow)
   - Authorization sequence (protected endpoint access)
   - JWT token generation
   - Password hashing with salt
   - Role-based access control

### Angular Application Documents

7. **angular-architecture-overview.puml** - Angular app structure
   - Standalone components architecture
   - Core services (@core folder)
   - Pages/routes
   - Models (TypeScript interfaces)
   - Configuration files
   - Angular Material components used
   - Interceptor pipeline

8. **angular-component-routing.puml** - Component hierarchy
   - All routes and their paths
   - Component tree structure
   - Guards (AuthGuard)
   - Public vs protected routes
   - Component responsibilities
   - Future component structure

9. **angular-http-communication.puml** - Frontend-backend communication
   - HTTP request flow
   - Interceptor chain (JWT, Headers)
   - Observable patterns
   - Error handling
   - State management approach
   - API response format

### Integration Documents

10. **e2e-create-requirement.puml** - End-to-end create workflow
    - Complete flow from user interaction to database
    - All layers involved (UI → API → Handler → Database)
    - Validation pipeline in detail
    - Document structure stored in Couchbase
    - Success and error paths

11. **e2e-semantic-search.puml** - End-to-end search workflow
    - Vector embedding generation
    - Cosine similarity calculation
    - SQL Server vector query
    - Pagination handling
    - Result enrichment from Couchbase
    - Search parameters

12. **deployment-architecture.puml** - Deployment strategy
    - Development environment setup
    - Server architecture
    - Database deployment (Couchbase Lite embedded, SQL Server)
    - Configuration management
    - Build and run commands
    - Hosting options

## PlantUML Conventions Used

### General Styling

```plantuml
skinparam backgroundColor #FEFEFE
skinparam defaultFontSize 11
skinparam wrapWidth 250
skinparam maxMessageSize 200
```

- Light background (#FEFEFE) for clean appearance
- Font size 11 for readability
- Wrap width 250 to prevent horizontal overflow
- Max message size 200 for sequence diagrams

### Width Constraint

All diagrams MUST fit within **1200 pixels width** to ensure no horizontal scrolling on standard screens.

**Technique**: Use `scale` directive if diagram exceeds 1200px:
```plantuml
scale 0.95
```

### Color Schemes

**Packages and Components:**
- Presentation Layer: Light Blue (#E3F2FD, border #1976D2)
- Core/Domain Layer: Light Blue (#E3F2FD, border #1976D2)
- Infrastructure Layer: Light Blue (#E3F2FD, border #1976D2)
- Test Layer: Light Blue (#E3F2FD, border #1976D2)
- Angular packages: Light Purple (#F3E5F5, border #8E24AA)

**Databases:**
- Document Store (Couchbase): Light Green (#C8E6C9, border #388E3C)
- Vector Store (SQL Server): Light Green (#C8E6C9, border #388E3C)

**Sequence Diagrams:**
- Arrow Color: Blue (#1976D2)
- Background: Light Blue (#E3F2FD)
- Alt/Loop boxes: Automatic coloring

**Class Diagrams:**
- Class Background: Light Blue (#E1F5FE)
- Class Border: Dark Blue (#01579B)
- Enums: Different shading

### Component Notation

**Simple components** (to avoid parsing errors):
```plantuml
component "ComponentName" as alias
note right of alias
    **Title**
    Description
    
    - Detail 1
    - Detail 2
end note
```

**Avoid multi-line bracket notation** `[ ... ]` as it causes parsing errors with complex content.

### Relationship Notation

**Dependencies:**
```plantuml
A --> B : uses
A ..> B : depends on (dashed)
```

**Composition:**
```plantuml
A "1" *-- "0..*" B : contains
```

**Aggregation:**
```plantuml
A "0..1" o-- "0..*" B : has
```

**Inheritance:**
```plantuml
B --|> A : inherits
```

**Implementation:**
```plantuml
B ..|> A : implements
```

### Notes Usage

**Purpose:** Provide additional context, implementation details, and generation hints.

**Placement:**
```plantuml
note right of Component
note left of Component
note top of Component
note bottom of Component
```

**Content Format:**
```plantuml
note right of Component
  **Section Title**
  Description text explaining
  the purpose or behavior.
  
  Key details:
  - Point 1
  - Point 2
  
  Code generation hints:
  - Technology X
  - Pattern Y
end note
```

### Sequence Diagram Conventions

**Actors:**
```plantuml
actor "User" as user
```

**Participants with grouping:**
```plantuml
box "Layer Name" #ColorCode
    participant "Component" as alias
end box
```

**Activation:**
```plantuml
activate participant
...
deactivate participant
```

**Alternatives (if/else):**
```plantuml
alt Condition True
    ...
else Condition False
    ...
end
```

**Loops:**
```plantuml
loop For each item
    ...
end
```

### Class Diagram Conventions

**Class structure:**
```plantuml
class ClassName {
    +PublicProperty : Type
    -PrivateProperty : Type
    #ProtectedProperty : Type
    __
    +PublicMethod() : ReturnType
}
```

**Visibility:**
- `+` Public
- `-` Private
- `#` Protected

**Collections:**
```plantuml
+Items : List<ItemType>
```

**Optional/Nullable:**
```plantuml
+OptionalProperty : Type?
```

### Package Organization

```plantuml
package "Package Name" <<Rectangle>> {
    class/component definitions
}
```

Use `<<Rectangle>>` stereotype for clear visual separation.

## Code Generation Guidance

### From Architecture Diagrams

**dotnet-solution-architecture.puml** provides:
- Project names and types
- Dependencies for .csproj references
- Framework versions (ASP.NET Core 8.0, .NET 8.0)
- Project folder structure

**Generation Steps:**
1. Create solution file: `Tseten.sln`
2. Create project folders: `src/`, `test/`
3. Create projects with `dotnet new` commands:
   - `dotnet new webapi -n Tseten.Api`
   - `dotnet new classlib -n Tseten.Core`
   - `dotnet new classlib -n Tseten.Infrastructure`
   - `dotnet new xunit -n Tseten.Api.Tests`
   - `dotnet new xunit -n Tseten.Core.Tests`
   - `dotnet new xunit -n Tseten.Infrastructure.Tests`
4. Add project references based on arrows in diagram
5. Install NuGet packages noted in diagrams

### From Domain Model Diagrams

**dotnet-core-domain-models.puml** provides:
- Namespace organization
- All entity classes with properties and types
- Relationships (composition, aggregation, inheritance)
- Enumerations with all values
- Comments explaining purpose

**Generation Steps:**
1. Create namespace folders matching package names
2. Generate class files with all properties
3. Implement relationships as properties:
   - Composition: Child collection in parent
   - Aggregation: Reference property
   - Inheritance: Base class specification
4. Generate enum files

### From CQRS Pattern Diagrams

**dotnet-cqrs-pattern.puml** provides:
- All request classes with properties
- All response classes with properties
- Handler class names and methods
- Validator class names
- MediatR interface implementations

**Generation Steps:**
1. Create Request/Response classes
2. Implement `IRequest<TResponse>` on requests
3. Create Handler classes implementing `IRequestHandler<TRequest, TResponse>`
4. Create Validator classes using FluentValidation
5. Register MediatR in DI container

### From API Endpoint Diagrams

**dotnet-api-endpoints.puml** provides:
- Controller names and routes
- All endpoint methods with HTTP verbs
- Route parameters and query parameters
- Request/Response types
- Service dependencies

**Generation Steps:**
1. Create controller classes with `[ApiController]` and `[Route]` attributes
2. Inject `IMediator` in constructor
3. Create action methods with appropriate HTTP attributes
4. Map parameters to request objects
5. Send requests via MediatR
6. Return responses

### From Database Diagrams

**dotnet-database-architecture.puml** provides:
- Database names and types
- Collection/table names
- Schema structures (all columns/fields)
- Context class names and methods
- Connection string format

**Generation Steps:**
1. Create `ITsetenContext` interface
2. Implement `TsetenContext` for Couchbase Lite
3. Create `VectorDbContext : DbContext` for EF Core
4. Implement repository methods
5. Configure connection strings in appsettings.json
6. Create EF Core migrations for SQL Server

### From Angular Diagrams

**angular-architecture-overview.puml** provides:
- Folder structure (@core, pages, models)
- Service class names and methods
- Component names and selectors
- Interceptor implementations
- Configuration structure

**Generation Steps:**
1. Create Angular app: `ng new Tseten.App --standalone`
2. Create folders: `src/app/@core`, `src/app/pages`, `src/app/models`
3. Generate services: `ng generate service @core/auth`
4. Generate components: `ng generate component pages/login --standalone`
5. Create TypeScript interfaces in models
6. Implement interceptors
7. Configure app.config.ts with providers

**angular-component-routing.puml** provides:
- All routes with paths
- Component assignments
- Guard assignments
- Route redirects

**Generation Steps:**
1. Create `app.routes.ts`
2. Define Route array with all paths
3. Assign components to routes
4. Configure guards
5. Set up redirects

### From Sequence Diagrams

**e2e-create-requirement.puml** and **e2e-semantic-search.puml** provide:
- Method call sequences
- Parameter passing
- Error handling flows
- Business logic implementation
- Data transformations

**Generation Steps:**
1. Follow the sequence to implement each method
2. Pass parameters as shown
3. Implement validation checks
4. Handle success and error cases
5. Return appropriate responses

## Rendering PlantUML Diagrams

### Requirements

1. **Java Runtime**: Java 11 or higher
2. **PlantUML**: Download plantuml.jar from https://plantuml.com/download
3. **Graphviz**: Required for class diagrams (`sudo apt-get install graphviz`)

### Commands

**Render single diagram:**
```bash
java -jar plantuml.jar -tpng diagram.puml
```

**Render all diagrams:**
```bash
java -jar plantuml.jar -tpng *.puml
```

**Render to specific output folder:**
```bash
java -jar plantuml.jar -tpng -o /output/folder diagram.puml
```

**Check syntax without rendering:**
```bash
java -jar plantuml.jar -syntax diagram.puml
```

### Verification

After rendering, verify:
1. **Width**: All PNGs should be ≤ 1200px wide
   ```bash
   file diagram.png | grep "PNG image data"
   ```
2. **Readability**: Text should be clear at normal zoom
3. **No errors**: PlantUML should not report syntax errors

## Maintenance Guidelines

### When to Update PlantUML Docs

1. **New Features**: Add new components, classes, or flows
2. **Architecture Changes**: Update layer relationships
3. **API Changes**: Add/modify/remove endpoints
4. **Database Schema Changes**: Update entity properties
5. **Deployment Changes**: Modify deployment architecture

### Update Process

1. Edit the relevant .puml file(s)
2. Render to PNG to verify syntax and appearance
3. Check width constraint (≤ 1200px)
4. Update this guidelines document if conventions change
5. Commit both .puml and .png files

### Consistency Checklist

- [ ] Consistent color scheme across diagrams
- [ ] Same font size and styling
- [ ] Notes on all major components
- [ ] Clear relationship indicators
- [ ] Width under 1200px
- [ ] No syntax errors
- [ ] Descriptive diagram titles
- [ ] Proper namespace/package organization

## Document Completeness for Code Generation

The PlantUML documents in this folder are designed to be **sufficient for complete code generation**. An agent should be able to:

### For .NET Solution
- [ ] Create solution file with all projects
- [ ] Generate all domain classes with properties and types
- [ ] Generate all DTOs (requests, responses)
- [ ] Generate all controllers with endpoints
- [ ] Generate all MediatR handlers
- [ ] Generate all FluentValidation validators
- [ ] Set up database contexts (Couchbase, EF Core)
- [ ] Configure dependency injection
- [ ] Implement authentication/authorization
- [ ] Configure database connections

### For Angular Application
- [ ] Create Angular workspace with correct configuration
- [ ] Generate all services with methods
- [ ] Generate all components with templates
- [ ] Set up routing with guards
- [ ] Create TypeScript interfaces for models
- [ ] Implement HTTP interceptors
- [ ] Configure Angular Material
- [ ] Set up environment configurations

### Integration
- [ ] Understand end-to-end workflows
- [ ] Implement error handling patterns
- [ ] Set up proper communication between layers
- [ ] Deploy applications correctly

## Diagram Reading Order for Code Generation

**Recommended sequence for a coding agent:**

1. **Start with Solution Architecture** (dotnet-solution-architecture.puml)
   - Understand overall structure
   - Create projects and folders

2. **Domain Models** (dotnet-core-domain-models.puml)
   - Generate all entities and enums
   - Foundation for everything else

3. **CQRS Pattern** (dotnet-cqrs-pattern.puml)
   - Generate DTOs and handlers
   - Implement validation

4. **Database Architecture** (dotnet-database-architecture.puml)
   - Set up data access layer
   - Configure databases

5. **API Endpoints** (dotnet-api-endpoints.puml)
   - Generate controllers
   - Wire up endpoints

6. **Authentication** (dotnet-authentication-flow.puml)
   - Implement security
   - Set up JWT

7. **Angular Architecture** (angular-architecture-overview.puml)
   - Create Angular app structure
   - Generate core services

8. **Angular Components** (angular-component-routing.puml)
   - Generate pages and routing
   - Implement components

9. **HTTP Communication** (angular-http-communication.puml)
   - Implement API integration
   - Set up interceptors

10. **End-to-End Workflows** (e2e-*.puml)
    - Verify implementation completeness
    - Test integration points

11. **Deployment** (deployment-architecture.puml)
    - Configure for deployment
    - Set up databases

## Examples of Code Generation from Diagrams

### Example 1: Generate Entity from Domain Model

From this in the diagram:
```plantuml
class SoftwareRequirement {
    +SoftwareRequirementId : string
    +ParentSoftwareRequirementId : string
    +Description : string
    +CanImplement : bool
    +CanTest : bool
    +Comments : List<Comment>
    +AcceptanceCriteria : List<AcceptanceCriteria>
}
```

Generate this C# code:
```csharp
namespace Tseten.Models.SoftwareRequirement;

public class SoftwareRequirement
{
    public string SoftwareRequirementId { get; set; }
    public string ParentSoftwareRequirementId { get; set; }
    public string Description { get; set; }
    public bool CanImplement { get; set; }
    public bool CanTest { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<AcceptanceCriteria> AcceptanceCriteria { get; set; } = new();
}
```

### Example 2: Generate API Endpoint from Endpoint Diagram

From this in the diagram:
```
**POST /** - Create
Creates new software requirement
Body: CreateSoftwareRequirementRequest
Returns: CreateSoftwareRequirementResponse
```

Generate this C# code:
```csharp
[HttpPost(Name = "Create")]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> CreateAsync([FromBody]CreateSoftwareRequirementRequest request)
{
    var response = await _mediator.Send(request);
    return Ok(response);
}
```

### Example 3: Generate Route from Angular Routing Diagram

From this in the diagram:
```
Route: 'requirements'
→ RequirementsListPage
→ canActivate: [AuthGuard]
```

Generate this TypeScript code:
```typescript
{
  path: 'requirements',
  component: RequirementsList,
  canActivate: [AuthGuard]
}
```

## Version Information

- **PlantUML Version**: 1.2024.7
- **Documentation Created**: 2026-01-10
- **Tseten Version**: Based on commit at time of documentation
- **.NET Version**: 8.0
- **Angular Version**: 19

## Additional Resources

- **PlantUML Official Documentation**: https://plantuml.com/
- **PlantUML Sequence Diagrams**: https://plantuml.com/sequence-diagram
- **PlantUML Class Diagrams**: https://plantuml.com/class-diagram
- **PlantUML Component Diagrams**: https://plantuml.com/component-diagram
- **C4 Model**: https://c4model.com/

## Conclusion

These PlantUML documents serve as a complete architectural blueprint for the Tseten project. By following these guidelines and reading the diagrams in the suggested order, a coding agent can generate the entire solution from scratch with high fidelity to the original design.
