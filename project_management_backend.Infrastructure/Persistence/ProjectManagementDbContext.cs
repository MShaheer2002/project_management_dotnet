using Microsoft.EntityFrameworkCore;
using project_management_backend.Domain.Entities.Issues;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.Infrastructure.Persistence
{
    public class ProjectManagementDbContext : DbContext
    {
        public ProjectManagementDbContext(
            DbContextOptions<ProjectManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}