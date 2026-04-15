

using Microsoft.EntityFrameworkCore;
using project_management_backend.Domain.Entities.Department;
using project_management_backend.Domain.Entities.Issues;
using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;
using project_management_backend.Domain.Entities.Users;
using project_management_backend.Domain.Entities.Workspace;

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
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentMember> DepartmentMembers { get; set; }


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

            // Tell EF to use backing field
            modelBuilder.Entity<Organization>()
                .Navigation(o => o.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Team>()
                .Navigation(t => t.Members)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(o => o.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(o => o.Slug)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(o => o.Slug)
                    .IsUnique();
            });
            modelBuilder.Entity<Team>()
                    .HasMany(t => t.Members)
                    .WithOne(nameof(TeamMember.Team))
                    .HasForeignKey(nameof(TeamMember.TeamId))
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Owner)
                .WithMany() // Owner may have multiple organizations
                .HasForeignKey(o => o.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}