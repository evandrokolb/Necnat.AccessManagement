using Microsoft.EntityFrameworkCore;
using Necnat.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Necnat.Server.DbContexts
{
    public class NecnatAccessManagementDbContext : DbContext
    {
        public NecnatAccessManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Unique
            builder.Entity<Api>()
                .HasIndex(u => new { u.ControllerId, u.Name, u.HttpMethodId, u.Version })
                .IsUnique();

            //Unique
            builder.Entity<Application>()
                    .HasIndex(u => u.Name)
                    .IsUnique();

            //Cascade
            builder.Entity<ApplicationHierarchy>()
                .HasOne(e => e.Application)
                .WithMany(f => f.ApplicationHierarchyList)
                .HasForeignKey(g => g.HierarchyId)
                .OnDelete(DeleteBehavior.Restrict);

            //Restrict
            builder.Entity<ApplicationHierarchy>()
                .HasOne(e => e.Hierarchy)
                .WithMany(f => f.ApplicationHierarchyList)
                .HasForeignKey(g => g.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            //Unique
            builder.Entity<Controller>()
                .HasIndex(u => new { u.ModuleId, u.Name })
                .IsUnique();

            //Unique
            builder.Entity<Feature>()
                .HasIndex(u => new { u.ModuleId, u.CodeName })
                .IsUnique();

            //Cascade
            builder.Entity<FeatureApi>()
                .HasOne(e => e.Feature)
                .WithMany(f => f.FeatureApiList)
                .HasForeignKey(g => g.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);

            //Restrict
            builder.Entity<FeatureApi>()
                .HasOne(e => e.Api)
                .WithMany(f => f.FeatureApiList)
                .HasForeignKey(g => g.ApiId)
                .OnDelete(DeleteBehavior.Restrict);

            //Unique
            builder.Entity<Hierarchy>()
                .HasIndex(u => u.Name)
                .IsUnique();

            //Cascade
            builder.Entity<HierarchyHierarchyComponentType>()
                .HasOne(e => e.Hierarchy)
                .WithMany(f => f.HierarchyHierarchyComponentTypeList)
                .HasForeignKey(g => g.HierarchyId)
                .OnDelete(DeleteBehavior.Cascade);

            //Unique
            builder.Entity<HierarchyComponentType>()
                .HasIndex(u => u.Name)
                .IsUnique();

            //Cascade
            builder.Entity<HierarchicalStructure>()
                .HasMany(e => e.SecurityList)
                .WithOne(f => f.HierarchicalStructure)
                .HasForeignKey(g => g.HierarchicalStructureId)
                .OnDelete(DeleteBehavior.Cascade);

            //Unique
            builder.Entity<Module>()
                .HasIndex(u => new { u.ApplicationId, u.CodeName })
                .IsUnique();

            //Unique
            builder.Entity<Role>()
                .HasIndex(u => new { u.ApplicationId, u.CodeName })
                .IsUnique();

            //Unique
            builder.Entity<RoleFeature>()
                .HasIndex(u => new { u.FeatureId, u.RoleId })
                .IsUnique();

            //Restrict
            builder.Entity<RoleFeature>()
                .HasOne(e => e.Feature)
                .WithMany(f => f.RoleFeatureList)
                .HasForeignKey(g => g.FeatureId)
                .OnDelete(DeleteBehavior.Restrict);

            //Cascade
            builder.Entity<RoleFeature>()
                .HasOne(e => e.Role)
                .WithMany(f => f.RoleFeatureList)
                .HasForeignKey(g => g.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            //Unique
            builder.Entity<Security>()
                .HasIndex(u => new { u.UserId, u.RoleId, u.HierarchicalStructureId })
                .IsUnique();
        }

        public DbSet<Api> Api { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<ApplicationHierarchy> ApplicationHierarchy { get; set; }
        public DbSet<Controller> Controller { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<FeatureApi> FeatureApi { get; set; }
        public DbSet<Hierarchy> Hierarchy { get; set; }
        public DbSet<HierarchicalStructure> HierarchicalStructure { get; set; }
        public DbSet<HierarchyComponentType> HierarchyComponentType { get; set; }
        public DbSet<HierarchyHierarchyComponentType> HierarchyHierarchyComponentType { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleFeature> RoleFeature { get; set; }
        public DbSet<Security> Security { get; set; }
    }
}
