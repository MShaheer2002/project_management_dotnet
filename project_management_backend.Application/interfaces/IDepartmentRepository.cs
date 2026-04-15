using project_management_backend.Domain.Entities.Department;

namespace project_management_backend.Application.Interface
{
    public interface IDepartmentRepository
    {
        Task<Department> CreateAsync(string title, string? description, Guid orgId, Guid workspaceId, Guid createdBy, string? icon, CancellationToken cancellationToken);
        Task<Department> UpdateAsync(Guid DepartmentId, string title, string? description, Guid updatedBy, CancellationToken cancellationToken);
        Task<Department> ChangeLeaderAsync(Guid DepartmentId, Guid? leaderId, Guid updatedBy, CancellationToken cancellationToken);
        Task<Department> ActivateAsync(Guid DepartmentId, Guid updatedBy, CancellationToken cancellationToken);
        Task<Department> DeactivateAsync(Guid DepartmentId, Guid updatedBy, CancellationToken cancellationToken);
        Task<Department> SoftDeleteAsync(Guid DepartmentId, Guid deletedBy, CancellationToken cancellationToken);
        Task<Department> UpdateIconAsync(Guid DepartmentId, string icon, Guid updatedBy, CancellationToken cancellationToken);
        Task<Department> AddMemberAsync(Guid addedBy, Guid DepartmentId, Guid newDeparMemberUserId, DepartmentRole role, CancellationToken cancellationToken);
        Task<Department> RemoveMemberAsync(Guid DepartmentId, Guid Id, CancellationToken cancellationToken);

    }
}