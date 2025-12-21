# Validation Backend Specifications

## Overview

The Validation feature provides a cross-cutting concern implementation for request validation using FluentValidation integrated with MediatR pipeline. This ensures all incoming requests are validated before processing by handlers.

## Architecture

| Component | Technology | Purpose |
|-----------|------------|---------|
| Validation Library | FluentValidation 11.10.0 | Declarative validation rules |
| CQRS Pipeline | MediatR 12.4.1 | Request pipeline with behaviors |
| Response Base | Custom | Error collection for responses |

---

## Component Overview

### Project Structure

```
src/Tseten.Validation/
    ConfigureServices.cs      # DI registration extension
    ResponseBase.cs           # Base response with errors
    ValidationBehavior.cs     # MediatR pipeline behavior
```

---

## SPEC-VAL-001: Response Base Class

### Description
Provides a base class for all API responses that includes an errors collection for validation messages.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-001.1 | Response SHALL contain an Errors list property | Implemented |
| AC-001.2 | Errors list SHALL be initialized as empty by default | Implemented |
| AC-001.3 | All response types SHALL inherit from ResponseBase | Implemented |

### Implementation

```csharp
namespace Tseten.Validation;

public class ResponseBase
{
    public ResponseBase()
    {
        Errors = new List<string>();
    }
    public List<string> Errors { get; set; }
}
```

### Usage Pattern

All response DTOs inherit from ResponseBase:

```csharp
public class CreateSoftwareRequirementResponse : ResponseBase
{
    public SoftwareRequirementDto SoftwareRequirement { get; set; }
}

public class GetSoftwareRequirementsResponse : ResponseBase
{
    public List<SoftwareRequirementDto> SoftwareRequirements { get; set; }
}
```

### Response Structure

| Property | Type | Description |
|----------|------|-------------|
| Errors | List of string | Collection of validation error messages |

---

## SPEC-VAL-002: Validation Pipeline Behavior

### Description
Implements a MediatR pipeline behavior that intercepts all requests, validates them using FluentValidation validators, and short-circuits the pipeline if validation fails.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-002.1 | System SHALL intercept all MediatR requests | Implemented |
| AC-002.2 | System SHALL execute all registered validators for request type | Implemented |
| AC-002.3 | System SHALL collect all validation failures | Implemented |
| AC-002.4 | System SHALL return response with errors if validation fails | Implemented |
| AC-002.5 | System SHALL proceed to handler if validation passes | Implemented |
| AC-002.6 | System SHALL support multiple validators per request type | Implemented |

### Implementation

```csharp
namespace Tseten.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(validationFailure => validationFailure != null)
            .ToList();

        if (failures.Any())
        {
            var response = new TResponse();

            foreach (var failure in failures)
            {
                response.Errors.Add(failure.ErrorMessage);
            }

            return response;
        }

        return await next();
    }
}
```

### Pipeline Flow

```
Request Received
      |
      v
+---------------------+
| ValidationBehavior  |
|---------------------|
| 1. Create context   |
| 2. Run validators   |
| 3. Collect failures |
+---------------------+
      |
      v
  [Has Errors?]
    /     \
   Yes     No
   /         \
  v           v
Return      Continue to
Response    Next Handler
with Errors     |
                v
           +----------+
           | Handler  |
           +----------+
                |
                v
            Response
```

### Type Constraints

| Constraint | Purpose |
|------------|---------|
| `TRequest : IRequest<TResponse>` | Ensures request is MediatR compatible |
| `TResponse : ResponseBase, new()` | Ensures response has errors collection and can be instantiated |

### Validation Context

The behavior creates a `ValidationContext<TRequest>` which provides:
- Access to the request instance
- Support for custom validation context data
- Integration with FluentValidation features

---

## SPEC-VAL-003: Dependency Injection Configuration

### Description
Provides extension method for registering validation services in the DI container.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-003.1 | System SHALL register ValidationBehavior as pipeline behavior | Implemented |
| AC-003.2 | System SHALL auto-discover validators from assembly | Implemented |
| AC-003.3 | System SHALL register behaviors with transient lifetime | Implemented |

### Implementation

```csharp
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddValidation(this IServiceCollection services, Type type)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssemblyContaining(type);
    }
}
```

### Usage in Program.cs

```csharp
builder.Services.AddValidation(typeof(SoftwareRequirement));
```

### Registration Details

| Registration | Lifetime | Purpose |
|--------------|----------|---------|
| `IPipelineBehavior<,>` | Transient | Pipeline interception |
| Validators | Transient (default) | Request validation |

---

## SPEC-VAL-004: Validator Implementation Pattern

### Description
Standard pattern for implementing FluentValidation validators for request classes.

### Acceptance Criteria

| ID | Criteria | Status |
|----|----------|--------|
| AC-004.1 | Validators SHALL inherit from AbstractValidator | Implemented |
| AC-004.2 | Validators SHALL define rules in constructor | Implemented |
| AC-004.3 | Validators SHALL use fluent syntax for rule definitions | Implemented |

### Standard Validator Pattern

```csharp
public class CreateSoftwareRequirementRequestValidator
    : AbstractValidator<CreateSoftwareRequirementRequest>
{
    public CreateSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
    }
}
```

### Available Validation Rules

| Rule | Description |
|------|-------------|
| `NotNull()` | Value must not be null |
| `NotEmpty()` | Value must not be null or empty string |
| `MaximumLength(n)` | String must not exceed n characters |
| `MinimumLength(n)` | String must be at least n characters |
| `Must(predicate)` | Custom validation logic |
| `When(condition)` | Conditional validation |

### Existing Validators

| Validator | Rules |
|-----------|-------|
| CreateSoftwareRequirementRequestValidator | Description: NotNull, NotEmpty |
| UpdateSoftwareRequirementRequestValidator | SoftwareRequirementId, ParentSoftwareRequirementId, Description: NotNull, NotEmpty |
| DeleteSoftwareRequirementRequestValidator | SoftwareRequirementId: NotNull, NotEmpty |
| CreateTagRequestValidator | Name, Description: NotNull, NotEmpty |
| UpdateTagRequestValidator | TagId: NotNull; Name, Description: NotNull, NotEmpty |
| DeleteTagRequestValidator | TagId: NotNull |

---

## Error Message Format

### Default Error Messages

FluentValidation generates default error messages based on the rule:

| Rule | Default Message |
|------|-----------------|
| NotNull | 'PropertyName' must not be empty |
| NotEmpty | 'PropertyName' must not be empty |

### Error Response Example

```json
{
    "softwareRequirement": null,
    "errors": [
        "'Description' must not be empty."
    ]
}
```

### Multiple Errors Example

```json
{
    "tag": null,
    "errors": [
        "'Name' must not be empty.",
        "'Description' must not be empty."
    ]
}
```

---

## Integration Points

### MediatR Pipeline Integration

```
Client Request
      |
      v
  Controller
      |
      v
_mediator.Send(request)
      |
      v
+-------------------+
| Pipeline Start    |
+-------------------+
      |
      v
+-------------------+
| ValidationBehavior|  <-- Intercepts here
+-------------------+
      |
      v
+-------------------+
| Other Behaviors   |
+-------------------+
      |
      v
+-------------------+
| Request Handler   |
+-------------------+
      |
      v
   Response
```

### Controller Error Handling

Controllers return the response directly, letting the client interpret errors:

```csharp
[HttpPost(Name = "Create")]
public async Task<IActionResult> CreateAsync([FromBody] CreateSoftwareRequirementRequest request)
{
    var response = await _mediator.Send(request);
    return Ok(response);  // Returns with errors if validation failed
}
```

---

## Technical Notes

- Validation occurs synchronously using `Validate()` not `ValidateAsync()`
- Multiple validators for the same request type are all executed
- Errors are aggregated from all validators
- Short-circuit prevents handler execution on validation failure
- Response base requires parameterless constructor (`new()` constraint)
- Validators are automatically discovered via assembly scanning
