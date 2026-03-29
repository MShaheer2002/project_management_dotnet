using project_management_backend.Domain.Entities.Workspace;

namespace project_management_backend.Application.Interface
{
    public interface IWorkspaceRepository
    {
        // CREATE
        Task<Workspace> CreateAsync(Workspace workspace, CancellationToken cancellationToken);

        // READ
        Task<Workspace?> GetByIdAsync(Guid workspaceId, CancellationToken cancellationToken);

        Task<Workspace?> GetWorkspaceWithMembersAsync(Guid workspaceId, CancellationToken cancellationToken);

        Task<List<Workspace>> GetByOrganizationAsync(
            Guid organizationId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<List<Workspace>> GetUserWorkspacesAsync(
            Guid organizationMemberId,
            CancellationToken cancellationToken);

        Task<Workspace?> GetByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Guid workspaceId, CancellationToken cancellationToken);

        Task<bool> ExistsByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken);

        Task<bool> IsUserInWorkspaceAsync(Guid workspaceId, Guid organizationMemberId, CancellationToken cancellationToken);

        // UPDATE
        Task UpdateAsync(Workspace workspace, CancellationToken cancellationToken);

        // DELETE
        Task DeleteAsync(Guid workspaceId, CancellationToken cancellationToken);
    }
}