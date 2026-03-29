using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Workspace;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public WorkspaceRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Workspace> CreateAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            await dbContext.Workspace.AddAsync(workspace, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return workspace;
        }

        public Task DeleteAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            var exist = await dbContext.Workspace.FindAsync(workspaceId, cancellationToken);

            return exist == null ? false : true;
        }

        public Task<bool> ExistsByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Workspace?> GetByIdAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Workspace?> GetByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Workspace>> GetByOrganizationAsync(Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Workspace>> GetUserWorkspacesAsync(Guid organizationMemberId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Workspace?> GetWorkspaceWithMembersAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInWorkspaceAsync(Guid workspaceId, Guid organizationMemberId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}