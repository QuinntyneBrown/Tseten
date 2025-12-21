// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tseten.Api.Features.User;
using Tseten.Core;

namespace Tseten.Api.Tests.Handlers;

public class AuthenticateHandlerTests
{
    private readonly Mock<ITsetenContext> _mockContext;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ILogger<AuthenticateHandler>> _mockLogger;

    public AuthenticateHandlerTests()
    {
        _mockContext = new Mock<ITsetenContext>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockTokenService = new Mock<ITokenService>();
        _mockLogger = new Mock<ILogger<AuthenticateHandler>>();
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnTokens()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4 };
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "test@example.com",
            Password = "hashedPassword",
            Salt = salt,
            Roles = new List<Role>()
        };

        var users = new List<User> { user }.AsQueryable();
        var mockUsersDbSet = CreateMockDbSet(users);
        _mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        _mockPasswordHasher.Setup(p => p.VerifyPassword("correctPassword", "hashedPassword", salt))
            .Returns(true);

        _mockTokenService.Setup(t => t.GenerateAccessToken(It.IsAny<User>()))
            .Returns("accessToken123");
        _mockTokenService.Setup(t => t.GenerateRefreshToken())
            .Returns("refreshToken456");

        var handler = new AuthenticateHandler(
            _mockContext.Object,
            _mockPasswordHasher.Object,
            _mockTokenService.Object,
            _mockLogger.Object);

        var request = new AuthenticateRequest
        {
            Username = "test@example.com",
            Password = "correctPassword"
        };

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.UserId.Should().Be(user.UserId);
        response.Token.Should().Be("accessToken123");
        response.RefreshToken.Should().Be("refreshToken456");
        response.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldReturnError()
    {
        // Arrange
        var salt = new byte[] { 1, 2, 3, 4 };
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "test@example.com",
            Password = "hashedPassword",
            Salt = salt,
            Roles = new List<Role>()
        };

        var users = new List<User> { user }.AsQueryable();
        var mockUsersDbSet = CreateMockDbSet(users);
        _mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        _mockPasswordHasher.Setup(p => p.VerifyPassword("wrongPassword", "hashedPassword", salt))
            .Returns(false);

        var handler = new AuthenticateHandler(
            _mockContext.Object,
            _mockPasswordHasher.Object,
            _mockTokenService.Object,
            _mockLogger.Object);

        var request = new AuthenticateRequest
        {
            Username = "test@example.com",
            Password = "wrongPassword"
        };

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Errors.Should().Contain("Invalid username or password");
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldReturnError()
    {
        // Arrange
        var users = new List<User>().AsQueryable();
        var mockUsersDbSet = CreateMockDbSet(users);
        _mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);

        var handler = new AuthenticateHandler(
            _mockContext.Object,
            _mockPasswordHasher.Object,
            _mockTokenService.Object,
            _mockLogger.Object);

        var request = new AuthenticateRequest
        {
            Username = "nonexistent@example.com",
            Password = "anyPassword"
        };

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Errors.Should().Contain("Invalid username or password");
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(data.Provider));
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

        return mockSet;
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(System.Linq.Expressions.Expression expression, CancellationToken cancellationToken = default)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(System.Linq.Expressions.Expression) })!
            .MakeGenericMethod(expectedResultType)
            .Invoke(this, new[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(expectedResultType)
            .Invoke(null, new[] { executionResult })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(System.Linq.Expressions.Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return ValueTask.FromResult(_inner.MoveNext());
    }

    public T Current => _inner.Current;
}
