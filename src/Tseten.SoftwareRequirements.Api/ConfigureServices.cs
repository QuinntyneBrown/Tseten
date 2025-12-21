// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Couchbase.Lite;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Tseten.SoftwareRequirements.Api;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureApiServices
{
    public static void AddApiServices(this IServiceCollection services, Action<CorsPolicyBuilder> configureCorsPolicyBuilder, string connectionString)
    {
        services.AddSingleton<ISoftwareRequirementsRepository,SoftwareRequirementsRepository>();
        services.AddControllers();

        var db = new Database("software-requirements");

        services.AddSingleton(db);

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
