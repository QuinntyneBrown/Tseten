// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureValidationServices
{
    public static void AddValidation(this IServiceCollection services, Type markerType)
    {
        services.AddValidatorsFromAssemblyContaining(markerType);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Tseten.Validation.ValidationBehavior<,>));
    }
}
