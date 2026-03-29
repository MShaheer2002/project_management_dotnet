using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public Task ActivateAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> CreateAsync(Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetByKeyAsync(string key, Guid organizationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetByOrganizationAsync(Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetProjectWithMembersAsync(Guid projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}