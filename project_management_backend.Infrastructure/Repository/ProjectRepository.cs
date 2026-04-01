using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Dto.Project;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public ProjectRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task ActivateAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) throw new NotImplementedException("No Project Found!");
            if (project.Status == ProjectStatus.Active) throw new Exception("Already Active");
            project.Activate();
            await dbContext.SaveChangesAsync();

        }

        public async Task<bool> ArchiveAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
            if (project == null)
            {
                return false;
                throw new NotImplementedException("No Project Found!");
            }
            if (project.Status == ProjectStatus.Archived)
            {
                return false;
                throw new Exception("Already Archive");
            }
            project.Archive();
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Project> CreateAsync(Project project, CancellationToken cancellationToken)
        {
            // 1. Validate Target Date
            if (project.TargetDate <= DateTime.UtcNow)
                throw new ArgumentException("Target date must be in the future.");

            // 2. Check Name uniqueness
            var nameExists = await dbContext.Projects
                .AnyAsync(p => p.Name.ToLower() == project.Name.ToLower(), cancellationToken);

            if (nameExists)
                throw new ArgumentException("Project name already exists.");

            // 3. Check Key uniqueness
            var keyExists = await dbContext.Projects
                .AnyAsync(p => p.Key.ToLower() == project.Key.ToLower(), cancellationToken);

            if (keyExists)
                throw new ArgumentException("Project key already exists.");

            // 4. Save
            await dbContext.Projects.AddAsync(project, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return project;
        }

        public async Task DeleteAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
            if (project == null) throw new NotImplementedException("No Project Found!");

            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await dbContext.Projects.AnyAsync(P => P.Id == projectId, cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await dbContext.Projects.Include(p => p.Members).ThenInclude(p => p.User).FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        }

        public async Task<Project?> GetByKeyAsync(string key, Guid organizationId, CancellationToken cancellationToken)
        {
            return await dbContext.Projects.FirstOrDefaultAsync(p => p.OrganizationId == organizationId && p.Key == key, cancellationToken);
        }

        public async Task<List<Project>> GetByOrganizationAsync(Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var projects = await dbContext.Projects
                .Include(m => m.Members)
                .ThenInclude(m => m.User)
                .Where(p => p.OrganizationId == organizationId)
                .OrderBy(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return projects;
        }

        public async Task<Project?> GetProjectWithMembersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.Include(m => m.Members).FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);

            return project;
        }

        public async Task UpdateAsync(Guid projectId, string name, string description, DateTime targetDate, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
            if (project == null) throw new NotImplementedException("No Project Found!");
            project.UpdateDetails(name, description, targetDate);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}