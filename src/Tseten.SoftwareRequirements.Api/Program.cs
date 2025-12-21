// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core.Model.SoftwareRequirement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidation(typeof(SoftwareRequirement));

builder.Services.AddApiServices(corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins(builder.Configuration["WithOrigins"]!.Split(','));
}, builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();