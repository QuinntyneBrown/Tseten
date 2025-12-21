<div align="center">
  <img src="assets/logo.png" alt="Tseten Logo" width="200"/>
  <h1>Tseten</h1>
  <p>A full-stack software requirements management system with semantic search capabilities.</p>
</div>

## Overview

Tseten is a modern requirements management platform built with ASP.NET Core 8.0 and Angular 19. It provides comprehensive CRUD operations for software requirements, hierarchical commenting, BDD-style acceptance criteria, and advanced semantic search powered by vector embeddings.

## Project Structure

```
Tseten/
├── src/
│   ├── Tseten.Api/              # ASP.NET Core 8.0 REST API
│   ├── Tseten.Models/           # Shared domain models and DTOs
│   ├── Tseten.Validation/       # Cross-cutting validation framework
│   └── Tseten.App/              # Angular 19 web application
├── test/
│   └── Tseten.Api.Tests/        # xUnit tests with FluentAssertions
├── docs/                         # Architecture diagrams and specifications
└── endpoint.json                # API endpoint configuration
```

## Key Technologies

### Backend
- **Framework**: ASP.NET Core 8.0
- **Architecture**: CQRS via MediatR
- **Validation**: FluentValidation
- **Document Storage**: Couchbase.Lite
- **Vector Database**: SQL Server/SQL Express with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI

### Frontend
- **Framework**: Angular 19
- **Language**: TypeScript 5.6
- **Testing**: Jasmine (unit), Playwright (E2E)
- **Build Tool**: Angular CLI 19

## Features

### Software Requirements Management
- Full CRUD operations for software requirements
- Hierarchical parent-child requirement relationships
- Nested commenting system with threading support
- BDD-style acceptance criteria (Given-When-Then)
- Flags for implementability and testability

### Semantic Search
- Vector embedding-based semantic search
- Cosine similarity ranking with pagination
- Configurable similarity thresholds
- Optional filtering by implementability and testability
- Result highlighting with context snippets

### Data Storage
- **Document Store**: Couchbase.Lite for requirement documents
- **Vector Store**: SQL Server for embedding vectors (384-dimensional)
- Dual database strategy for optimal performance

## Getting Started

### Backend (API)

1. Navigate to the API project:
   ```bash
   cd src/Tseten.Api
   ```

2. Update the connection string in `appsettings.json` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TsetenVectorDb;..."
     }
   }
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7157` with Swagger UI at `/swagger`.

### Frontend (Angular App)

1. Navigate to the app directory:
   ```bash
   cd src/Tseten.App
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   ng serve
   ```

The application will be available at `http://localhost:4200`.

## API Endpoints

- `POST /api/softwarerequirements` - Create requirement
- `GET /api/softwarerequirements` - Get all requirements
- `GET /api/softwarerequirements/{id}` - Get requirement by ID
- `PUT /api/softwarerequirements` - Update requirement
- `DELETE /api/softwarerequirements/{id}` - Delete requirement
- `GET /api/softwarerequirements/search` - Semantic search with pagination
- `POST /api/softwarerequirements/embeddings/import` - Import embeddings

## Testing

### Backend Tests
```bash
cd test/Tseten.Api.Tests
dotnet test
```

### Frontend Tests
```bash
cd src/Tseten.App

# Unit tests
npm test

# E2E tests
npm run e2e
```

## Architecture

The project follows clean architecture principles with clear separation of concerns:

- **API Layer**: Controllers handling HTTP requests
- **Application Layer**: CQRS handlers with MediatR
- **Domain Layer**: Entities and business logic
- **Data Access Layer**: Repository pattern with dual database strategy
- **Validation Layer**: FluentValidation with MediatR pipeline behaviors

## Documentation

Detailed specifications and architecture diagrams are available in the [docs](docs/) directory:
- Backend specifications for CRUD, search, tags, and validation
- C4 model diagrams
- Sequence and class diagrams
- PlantUML architecture visualizations

## License

[Add your license information here]