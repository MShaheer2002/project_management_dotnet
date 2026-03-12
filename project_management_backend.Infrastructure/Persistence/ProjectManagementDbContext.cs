using Microsoft.EntityFrameworkCore;
using project_management_backend.Domain.Entities.Issues;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Domain.Entities.User;

namespace project_management_backend.Infrastructure.Persistence
{
    public class ProjectManagementDbContext : DbContext
    {
        public ProjectManagementDbContext(
            DbContextOptions<ProjectManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationMember> OrganizationMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }

        // Fluent API configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // OrganizationMember unique constraint
            modelBuilder.Entity<OrganizationMember>()
                .HasIndex(om => new { om.UserId, om.OrganizationId })
                .IsUnique();

            // User ↔ OrganizationMember
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.User)
                .WithMany(u => u.OrganizationMembers)
                .HasForeignKey(om => om.UserId);

            // Organization ↔ OrganizationMember
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.Organization)
                .WithMany(o => o.Members)
                .HasForeignKey(om => om.OrganizationId);
        }
    }
}