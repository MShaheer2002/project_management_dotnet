using Microsoft.EntityFrameworkCore;
using project_management_backend.Domain.Issues;
using project_management_backend.Domain.Organization;
using project_management_backend.Domain.Project;

namespace project_management_backend.Infrastructure.Data
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