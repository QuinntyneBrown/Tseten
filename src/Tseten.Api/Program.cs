// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Models.SoftwareRequirement;
using Tseten.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidation(typeof(SoftwareRequirement));

builder.Services.AddApiServices(corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins(builder.Configuration["WithOrigins"]!.Split(','));
}, builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure vector database is created
using (var scope = app.Services.CreateScope())
{
    var vectorDbContext = scope.ServiceProvider.GetRequiredService<VectorDbContext>();
    await vectorDbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();