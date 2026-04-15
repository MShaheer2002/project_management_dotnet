using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Department;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class DepartmentMemberRepository : IDepartmentMemberRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public DepartmentMemberRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<DepartmentMember> ChangeRoleAsync(Guid DepartmentId, Guid DepartmentMemberId, DepartmentRole role, Guid updatedBy, CancellationToken cancellationToken)
        {
            var departMember = await dbContext.DepartmentMembers.FirstOrDefaultAsync(dm => dm.Id == DepartmentMemberId && dm.DepartmentId == DepartmentId);
            if (departMember == null) throw new KeyNotFoundException("Departement  Member Not Found!");
            departMember.ChangeRole(role, updatedBy);
            return departMember;
        }

        public async Task<DepartmentMember> RemoveAsync(Guid DepartmentId, Guid DepartmentMemberId, Guid removedBy, CancellationToken cancellationToken)
        {
            var departMember = await dbContext.DepartmentMembers.FirstOrDefaultAsync(dm => dm.Id == DepartmentMemberId && dm.DepartmentId == DepartmentId);
            if (departMember == null) throw new KeyNotFoundException("Departement  Member Not Found!");
            departMember.Remove(removedBy);
            return departMember;
        }
    }
}