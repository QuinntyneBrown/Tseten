# Tseten PlantUML Documentation Index

## Quick Reference

This folder contains comprehensive PlantUML documentation for the Tseten software requirements management system. All diagrams are optimized for viewing on 1200px width screens without horizontal scrolling.

## Document Categories

### 🏗️ .NET Solution Architecture (6 documents)

| Document | Purpose | Key Information |
|----------|---------|-----------------|
| **dotnet-solution-architecture.puml** | Solution overview | Projects, layers, dependencies, databases |
| **dotnet-core-domain-models.puml** | Domain entities | All aggregates, properties, relationships, enums |
| **dotnet-cqrs-pattern.puml** | CQRS implementation | Requests, responses, handlers, validators |
| **dotnet-api-endpoints.puml** | REST API specification | All endpoints, routes, HTTP methods, parameters |
| **dotnet-database-architecture.puml** | Data storage | Couchbase Lite, SQL Server, schemas, contexts |
| **dotnet-authentication-flow.puml** | Security | JWT authentication, authorization, password hashing |

### 🅰️ Angular Application (3 documents)

| Document | Purpose | Key Information |
|----------|---------|-----------------|
| **angular-architecture-overview.puml** | App structure | Services, components, interceptors, Material UI |
| **angular-component-routing.puml** | Navigation | Routes, components, guards, hierarchy |
| **angular-http-communication.puml** | API integration | HTTP flow, interceptors, state management |

### 🔄 Integration & Deployment (3 documents)

| Document | Purpose | Key Information |
|----------|---------|-----------------|
| **e2e-create-requirement.puml** | Create workflow | Full stack flow from UI to database |
| **e2e-semantic-search.puml** | Search workflow | Vector embeddings, similarity search |
| **deployment-architecture.puml** | Deployment | Hosting, configuration, setup steps |

## Technology Stack Reference

### Backend (.NET)
- **Framework**: ASP.NET Core 8.0
- **Language**: C# with .NET 8.0
- **Pattern**: CQRS via MediatR
- **Validation**: FluentValidation
- **Document DB**: Couchbase Lite (embedded)
- **Vector DB**: SQL Server/SQL Express with EF Core
- **Authentication**: JWT Bearer tokens
- **Testing**: xUnit, FluentAssertions

### Frontend (Angular)
- **Framework**: Angular 19
- **Language**: TypeScript 5.6
- **Architecture**: Standalone Components
- **UI Library**: Angular Material 19
- **HTTP**: HttpClient with interceptors
- **Testing**: Jasmine (unit), Playwright (E2E)
- **Build**: Angular CLI 19

## Quick Start for Code Generation

**Order of reading for maximum efficiency:**

1. 📋 **dotnet-solution-architecture.puml** - Start here for project structure
2. 🧱 **dotnet-core-domain-models.puml** - Generate all entities and models
3. 📨 **dotnet-cqrs-pattern.puml** - Create DTOs, handlers, validators
4. 💾 **dotnet-database-architecture.puml** - Set up data access layer
5. 🌐 **dotnet-api-endpoints.puml** - Generate controllers and routes
6. 🔐 **dotnet-authentication-flow.puml** - Implement authentication/authorization
7. 🅰️ **angular-architecture-overview.puml** - Create Angular structure
8. 🧩 **angular-component-routing.puml** - Generate components and routes
9. 🔗 **angular-http-communication.puml** - Implement API communication
10. ✅ **e2e-create-requirement.puml** - Verify create workflow
11. 🔍 **e2e-semantic-search.puml** - Verify search workflow
12. 🚀 **deployment-architecture.puml** - Configure deployment

## Key Features Documented

### Software Requirements Management
- ✅ CRUD operations for requirements
- ✅ Hierarchical parent-child relationships
- ✅ Nested commenting system
- ✅ BDD-style acceptance criteria (Given-When-Then)
- ✅ Implementability and testability flags

### Semantic Search
- ✅ Vector embedding generation (384 dimensions)
- ✅ Cosine similarity ranking
- ✅ Pagination support
- ✅ Configurable similarity threshold
- ✅ Filtering by implementability/testability

### User Management
- ✅ JWT authentication
- ✅ Role-based authorization
- ✅ Password hashing with salt
- ✅ User profiles
- ✅ Invitation tokens

### Tags
- ✅ Tag creation and management
- ✅ Tag categorization for requirements

## Diagram Specifications

### Visual Standards
- **Width**: ≤ 1200px (optimized for standard screens)
- **Background**: Light (#FEFEFE)
- **Font Size**: 11pt
- **Color Scheme**: Consistent across all diagrams
  - Blue tones for .NET components
  - Purple tones for Angular components
  - Green for databases
  - Sequence diagrams use blue theme

### Notation Standards
- **→** Solid arrow: Usage/calls
- **..>** Dashed arrow: Depends on
- **\*--** Composition: Contains/owns
- **o--** Aggregation: Has/references
- **--|>** Inheritance: Extends
- **..|>** Implementation: Implements

## File Formats

Each diagram exists in two formats:
- **.puml** - Source PlantUML text file (editable)
- **.png** - Rendered image (for viewing)

## Rendering Instructions

### Prerequisites
```bash
# Java 11+ required
java -version

# Install Graphviz (for class diagrams)
sudo apt-get install graphviz

# Download PlantUML
wget https://github.com/plantuml/plantuml/releases/download/v1.2024.7/plantuml-1.2024.7.jar
```

### Render All Diagrams
```bash
java -jar plantuml.jar -tpng *.puml
```

### Render Single Diagram
```bash
java -jar plantuml.jar -tpng diagram-name.puml
```

### Verify Syntax
```bash
java -jar plantuml.jar -syntax diagram-name.puml
```

## Project Structure Depicted

```
Tseten/
├── src/
│   ├── Tseten.Api/           # ASP.NET Core 8.0 REST API
│   │   ├── Controllers/      # REST endpoint controllers
│   │   ├── Features/         # MediatR handlers (User, Tag)
│   │   ├── RequestHandlers/  # MediatR handlers (SoftwareRequirement)
│   │   ├── Services/         # Embedding service
│   │   └── Program.cs        # DI configuration
│   │
│   ├── Tseten.Core/          # Domain layer
│   │   ├── Model/            # All aggregates
│   │   ├── SoftwareRequirement/  # Req DTOs and validators
│   │   └── Services/         # Validation behavior
│   │
│   ├── Tseten.Infrastructure/    # Infrastructure layer
│   │   ├── TsetenContext.cs      # Couchbase Lite context
│   │   ├── VectorDbContext.cs    # EF Core context (not shown, but implied)
│   │   ├── PasswordHasher.cs     # Security
│   │   └── TokenService.cs       # JWT generation
│   │
│   └── Tseten.App/           # Angular 19 application
│       └── src/
│           ├── app/
│           │   ├── @core/    # Core services
│           │   ├── pages/    # Route components
│           │   └── models/   # TypeScript interfaces
│           └── environments/ # Config
│
└── test/
    ├── Tseten.Api.Tests/         # API integration tests
    ├── Tseten.Core.Tests/        # Core unit tests
    └── Tseten.Infrastructure.Tests/  # Infrastructure tests
```

## Database Schemas

### Couchbase Lite Collections

**software-requirements**
- SoftwareRequirementId (string, PK)
- ParentSoftwareRequirementId (string)
- Description (string)
- CanImplement (bool)
- CanTest (bool)
- Comments (array)
- AcceptanceCriteria (array)

**tags**
- TagId (Guid, PK)
- Name (string)
- Description (string)

**users**
- UserId (Guid, PK)
- Username (string)
- PasswordHash (string)
- Salt (string)
- Email (string)
- Roles (array)

**profiles**
- ProfileId (Guid, PK)
- UserId (Guid, FK)
- FirstName (string)
- LastName (string)
- Bio (string)
- AvatarUrl (string)

### SQL Server Tables

**Embeddings**
- Id (int, PK, Identity)
- SoftwareRequirementId (string)
- Embedding (float[], 384 dimensions)
- LastUpdated (DateTime)

## API Endpoints Summary

### Software Requirements
- `POST /api/softwarerequirements` - Create
- `GET /api/softwarerequirements` - Get all
- `GET /api/softwarerequirements/{id}` - Get by ID
- `PUT /api/softwarerequirements` - Update
- `DELETE /api/softwarerequirements/{id}` - Delete
- `GET /api/softwarerequirements/search` - Semantic search
- `POST /api/softwarerequirements/embeddings/import` - Sync embeddings

### Tags
- `POST /api/tags` - Create
- `GET /api/tags` - Get all
- `GET /api/tags/{id}` - Get by ID
- `PUT /api/tags` - Update
- `DELETE /api/tags/{id}` - Delete

### Users
- `POST /api/users/authenticate` - Login (get JWT)
- `POST /api/users` - Create user
- `POST /api/users/change-password` - Change password
- `GET /api/users` - Get all (admin)
- `GET /api/users/{id}` - Get by ID
- `GET /api/users/current` - Get current user
- `GET /api/users/exists` - Check username

## Angular Routes

- `/` - Redirect to /login
- `/login` - Login page (public)
- `/workspace` - Main workspace (protected)
- `/requirements` - Requirements list (protected)

## Development Setup Commands

### Backend
```bash
cd src/Tseten.Api
dotnet restore
dotnet run
# API runs on https://localhost:7157
```

### Frontend
```bash
cd src/Tseten.App
npm install
ng serve
# App runs on http://localhost:4200
```

### Tests
```bash
# Backend tests
cd test/Tseten.Api.Tests
dotnet test

# Frontend tests
cd src/Tseten.App
npm test          # Unit tests
npm run e2e       # E2E tests
```

## Documentation Guidelines

See **README.md** in this folder for:
- Complete PlantUML conventions
- Code generation guidelines
- Maintenance procedures
- Rendering instructions
- Examples of code generation from diagrams

## Diagram Validation Checklist

Each diagram has been validated for:
- ✅ Correct PlantUML syntax
- ✅ Width ≤ 1200px
- ✅ Renders without errors
- ✅ Clear and readable text
- ✅ Consistent styling
- ✅ Comprehensive notes
- ✅ Sufficient detail for code generation

## Version Control

- All .puml source files are version controlled
- PNG files are generated from .puml sources
- Update both when making changes
- Document version: 2026-01-10

## Support

For questions about:
- **PlantUML syntax**: See official docs at https://plantuml.com/
- **Tseten architecture**: Read the diagrams in suggested order
- **Code generation**: Follow README.md guidelines
- **Rendering issues**: Check Java/Graphviz installation

---

**Last Updated**: 2026-01-10  
**PlantUML Version**: 1.2024.7  
**Total Diagrams**: 12 (6 .NET + 3 Angular + 3 Integration/Deployment)
