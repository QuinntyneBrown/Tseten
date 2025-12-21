// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Tseten.Core;
using Tseten.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tseten.Infrastructure.Tests;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly IConfiguration _configuration;

    public TokenServiceTests()
    {
        var configValues = new Dictionary<string, string?>
        {
            { "Jwt:Key", "YourSecureSecretKeyHere_MustBeAtLeast32CharactersLong_!" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpirationMinutes", "60" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        _tokenService = new TokenService(_configuration);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnNonEmptyToken()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token = _tokenService.GenerateAccessToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Should().NotBeNull();
        jwtToken.Issuer.Should().Be("TestIssuer");
        jwtToken.Audiences.Should().Contain("TestAudience");
    }

    [Fact]
    public void GenerateAccessToken_ShouldIncludeUserClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            Username = "testuser@example.com",
            Roles = new List<Role>()
        };

        // Act
        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser@example.com");
    }

    [Fact]
    public void GenerateAccessToken_ShouldIncludeRoleClaims()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser@example.com",
            Roles = new List<Role>
            {
                new Role
                {
                    RoleId = roleId,
                    Name = "Admin",
                    Privileges = new List<Privilege>()
                }
            }
        };

        // Act
        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
    }

    [Fact]
    public void GenerateAccessToken_ShouldIncludePrivilegeClaims()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser@example.com",
            Roles = new List<Role>
            {
                new Role
                {
                    RoleId = roleId,
                    Name = "Admin",
                    Privileges = new List<Privilege>
                    {
                        new Privilege
                        {
                            PrivilegeId = Guid.NewGuid(),
                            RoleId = roleId,
                            Aggregate = "User",
                            AccessRight = AccessRight.Read
                        }
                    }
                }
            }
        };

        // Act
        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == "privilege" && c.Value == "ReadUser");
    }

    [Fact]
    public void GenerateAccessToken_ShouldHaveCorrectExpiration()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var token = _tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(60), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnNonEmptyToken()
    {
        // Act
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Assert
        refreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnUniqueTokens()
    {
        // Act
        var token1 = _tokenService.GenerateRefreshToken();
        var token2 = _tokenService.GenerateRefreshToken();

        // Assert
        token1.Should().NotBe(token2);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64EncodedToken()
    {
        // Act
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Assert - Base64 strings should be decodable without exception
        Action decodeAction = () => Convert.FromBase64String(refreshToken);
        decodeAction.Should().NotThrow();
    }

    [Fact]
    public void GenerateAccessToken_WithDefaultExpirationMinutes_ShouldUse10080()
    {
        // Arrange - Configuration without ExpirationMinutes
        var configValues = new Dictionary<string, string?>
        {
            { "Jwt:Key", "YourSecureSecretKeyHere_MustBeAtLeast32CharactersLong_!" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var tokenService = new TokenService(config);
        var user = CreateTestUser();

        // Act
        var token = tokenService.GenerateAccessToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert - Default is 10080 minutes (7 days)
        jwtToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(10080), TimeSpan.FromMinutes(1));
    }

    private static User CreateTestUser()
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser@example.com",
            Roles = new List<Role>()
        };
    }
}
