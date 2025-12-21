// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Couchbase.Lite;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Tseten.Api;
using Tseten.Api.Data;
using Tseten.Api.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureApiServices
{
    public static void AddApiServices(this IServiceCollection services, Action<CorsPolicyBuilder> configureCorsPolicyBuilder, string connectionString)
    {
        services.AddSingleton<ISoftwareRequirementsRepository,SoftwareRequirementsRepository>();
        services.AddControllers();

        // Couchbase.Lite for document storage
        var db = new Database("tseten-software-requirements");
        services.AddSingleton(db);

        // SQL Express for vector database
        services.AddDbContext<VectorDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            }));

        // Register embedding services
        services.AddSingleton<IEmbeddingService, EmbeddingService>();
        services.AddScoped<IEmbeddingImportService, EmbeddingImportService>();

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
