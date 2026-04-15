using project_management_backend.Domain.Entities.Department;

namespace project_management_backend.Application.Interface
{
    public interface IDepartmentMemberRepository
    {
        public Task<DepartmentMember> ChangeRoleAsync(Guid DepartmentId, Guid DepartmentMemberId, DepartmentRole role, Guid updatedBy, CancellationToken cancellationToken);
        public Task<DepartmentMember> RemoveAsync(Guid DepartmentId, Guid DepartmentMemberId, Guid removedBy, CancellationToken cancellationToken);


    }
}