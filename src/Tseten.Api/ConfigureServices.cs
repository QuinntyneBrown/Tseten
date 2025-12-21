// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Couchbase.Lite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tseten.Api;
using Tseten.Api.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureApiServices
{
    public static void AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<CorsPolicyBuilder> configureCorsPolicyBuilder)
    {
        services.AddSingleton<ISoftwareRequirementsRepository, SoftwareRequirementsRepository>();
        services.AddControllers();
        services.AddHttpContextAccessor();

        // Couchbase.Lite for document storage
        var db = new Database("tseten-software-requirements");
        services.AddSingleton(db);

        // Register embedding services
        services.AddSingleton<IEmbeddingService, EmbeddingService>();
        services.AddScoped<IEmbeddingImportService, EmbeddingImportService>();

        // JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                configureCorsPolicyBuilder.Invoke(builder);
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(isOriginAllowed: _ => true)
                    .AllowCredentials();
            }));

        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());
    }
}
