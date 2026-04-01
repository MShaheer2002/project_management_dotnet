using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.Application.Interface
{
    public interface IProjectRepository
    {
        // CREATE
        Task<Project> CreateAsync(Project project, CancellationToken cancellationToken);

        // READ
        Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken);

        Task<Project?> GetByKeyAsync(string key, Guid organizationId, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Guid projectId, CancellationToken cancellationToken);

        Task<List<Project>> GetByOrganizationAsync(
            Guid organizationId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        // UPDATE
        Task UpdateAsync(Guid projectId, string name, string description, DateTime targetDate, CancellationToken cancellationToken);

        // DELETE (soft delete preferred)
        Task DeleteAsync(Guid projectId, CancellationToken cancellationToken);

        // STATUS MANAGEMENT
        Task ActivateAsync(Guid projectId, CancellationToken cancellationToken);
        Task<bool> ArchiveAsync(Guid projectId, CancellationToken cancellationToken);

        // OPTIONAL (but useful for your UI)
        Task<Project?> GetProjectWithMembersAsync(Guid projectId, CancellationToken cancellationToken);
    }
}   