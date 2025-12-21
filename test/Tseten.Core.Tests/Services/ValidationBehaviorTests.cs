// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using Tseten.Core;

namespace Tseten.Core.Tests.Services;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithNoValidators_ShouldCallNext()
    {
        // Arrange
        var validators = Array.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "test" };
        var expectedResponse = new TestResponse { Result = "success" };

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCallNext()
    {
        // Arrange
        var validator = new TestRequestValidator();
        var validators = new IValidator<TestRequest>[] { validator };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "valid" };
        var expectedResponse = new TestResponse { Result = "success" };

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var validator = new TestRequestValidator();
        var validators = new IValidator<TestRequest>[] { validator };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "" }; // Invalid - empty value

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse());

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => behavior.Handle(request, next, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithMultipleValidators_ShouldValidateWithAll()
    {
        // Arrange
        var validator1 = new TestRequestValidator();
        var validator2 = new TestRequestLengthValidator();
        var validators = new IValidator<TestRequest>[] { validator1, validator2 };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "ab" }; // Invalid - too short

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse());

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => behavior.Handle(request, next, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithMultipleValidatorsAllValid_ShouldCallNext()
    {
        // Arrange
        var validator1 = new TestRequestValidator();
        var validator2 = new TestRequestLengthValidator();
        var validators = new IValidator<TestRequest>[] { validator1, validator2 };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "valid value" }; // Valid - not empty and >= 5 chars
        var expectedResponse = new TestResponse { Result = "success" };

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassToValidators()
    {
        // Arrange
        var validator = new TestRequestValidator();
        var validators = new IValidator<TestRequest>[] { validator };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest { Value = "valid" };
        var expectedResponse = new TestResponse { Result = "success" };
        var cancellationToken = new CancellationToken();

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(request, next, cancellationToken);

        // Assert
        result.Should().Be(expectedResponse);
    }

    // Test classes
    public class TestRequest : IRequest<TestResponse>
    {
        public string Value { get; set; } = string.Empty;
    }

    public class TestResponse
    {
        public string Result { get; set; } = string.Empty;
    }

    public class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }

    public class TestRequestLengthValidator : AbstractValidator<TestRequest>
    {
        public TestRequestLengthValidator()
        {
            RuleFor(x => x.Value).MinimumLength(5);
        }
    }
}
