// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api.Data;

/// <summary>
/// Entity Framework DbContext for vector-based semantic search using SQL Server/SQL Express.
/// Provides storage for software requirement embeddings used in similarity search operations.
/// </summary>
public class VectorDbContext : DbContext
{
    public VectorDbContext(DbContextOptions<VectorDbContext> options) : base(options)
    {
    }

    public DbSet<SoftwareRequirementEmbedding> SoftwareRequirementEmbeddings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SoftwareRequirementEmbedding>(entity =>
        {
            entity.ToTable("SoftwareRequirementEmbeddings");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.SoftwareRequirementId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.EmbeddingVector)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.EmbeddingDimension)
                .HasDefaultValue(384);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.SoftwareRequirementId)
                .IsUnique();
        });
    }
}
