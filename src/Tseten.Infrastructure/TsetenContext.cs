// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tseten.Core;
using Tseten.Models.SoftwareRequirement;

namespace Tseten.Infrastructure;

public class TsetenContext : DbContext, ITsetenContext
{
    public TsetenContext(DbContextOptions<TsetenContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Privilege> Privileges => Set<Privilege>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<InvitationToken> InvitationTokens => Set<InvitationToken>();
    public DbSet<SoftwareRequirementEmbedding> SoftwareRequirementEmbeddings => Set<SoftwareRequirementEmbedding>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<SoftwareRequirement> SoftwareRequirements => Set<SoftwareRequirement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            entity.Property(e => e.Username).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Salt).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity("UserRoles");
            entity.HasMany(e => e.Profiles)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasMany(e => e.Privileges)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.ToTable("Privileges");
            entity.HasKey(e => e.PrivilegeId);
            entity.Property(e => e.PrivilegeId).ValueGeneratedOnAdd();
            entity.Property(e => e.Aggregate).IsRequired().HasMaxLength(100);
            entity.Property(e => e.AccessRight).IsRequired();
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(e => e.ProfileId);
            entity.Property(e => e.ProfileId).ValueGeneratedOnAdd();
            entity.Property(e => e.Firstname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Lastname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<InvitationToken>(entity =>
        {
            entity.ToTable("InvitationTokens");
            entity.HasKey(e => e.InvitationTokenId);
            entity.Property(e => e.InvitationTokenId).ValueGeneratedOnAdd();
            entity.Property(e => e.Value).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Value).IsUnique();
            entity.Property(e => e.Type).IsRequired();
        });

        modelBuilder.Entity<SoftwareRequirementEmbedding>(entity =>
        {
            entity.ToTable("SoftwareRequirementEmbeddings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.SoftwareRequirementId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.EmbeddingVector).IsRequired().HasColumnType("nvarchar(max)");
            entity.Property(e => e.EmbeddingDimension).HasDefaultValue(384);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.SoftwareRequirementId).IsUnique();
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");
            entity.HasKey(e => e.TagId);
            entity.Property(e => e.TagId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<SoftwareRequirement>(entity =>
        {
            entity.ToTable("SoftwareRequirements");
            entity.HasKey(e => e.SoftwareRequirementId);
            entity.Property(e => e.SoftwareRequirementId).HasMaxLength(100);
            entity.Property(e => e.ParentSoftwareRequirementId).HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(4000);
            entity.HasMany(e => e.Comments)
                .WithOne()
                .HasForeignKey("SoftwareRequirementId")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.AcceptanceCriteria)
                .WithOne()
                .HasForeignKey("SoftwareRequirementId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(e => e.CommentId);
            entity.Property(e => e.CommentId).ValueGeneratedOnAdd();
            entity.Property(e => e.Body).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(256);
            entity.HasMany(e => e.Comments)
                .WithOne(e => e.ParentComment)
                .HasForeignKey(e => e.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AcceptanceCriteria>(entity =>
        {
            entity.ToTable("AcceptanceCriteria");
            entity.HasKey(e => e.AcceptanceCriteriaId);
            entity.Property(e => e.AcceptanceCriteriaId).ValueGeneratedOnAdd();
            entity.Property(e => e.Given).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.When).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Then).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
