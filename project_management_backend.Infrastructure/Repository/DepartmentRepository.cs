using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Department;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public DepartmentRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Department> ActivateAsync(Guid DepartmentId, Guid updatedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.Activate(updatedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> AddMemberAsync(Guid addedBy, Guid DepartmentId, Guid newDeparMemberUserId, DepartmentRole role, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            var orgMember = await dbContext.OrganizationMembers.FirstOrDefaultAsync(om => om.UserId == newDeparMemberUserId);
            if (orgMember == null) throw new KeyNotFoundException("User not found in Organization");
            var workspaceMember = await dbContext.WorkspaceMembers.FirstOrDefaultAsync(wm => wm.OrganizationMemberId == orgMember.Id);
            if (workspaceMember == null) throw new KeyNotFoundException("User not found in Workspace");

            var NewDepartmentMember = new DepartmentMember(DepartmentId, newDeparMemberUserId, workspaceMember.Id, role, addedBy);
            depart.AddMember(NewDepartmentMember);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> ChangeLeaderAsync(Guid DepartmentId, Guid? leaderId, Guid updatedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            if (depart.LeaderId == leaderId) throw new ArgumentException("Already a Leader");
            depart.ChangeLeader(leaderId, updatedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> CreateAsync(string title, string? description, Guid orgId, Guid workspaceId, Guid createdBy, string? icon, CancellationToken cancellationToken)
        {
            var depart = new Department(title, description, orgId, workspaceId, createdBy, icon);
            await dbContext.Departments.AddAsync(depart, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;

        }

        public async Task<Department> DeactivateAsync(Guid DepartmentId, Guid updatedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.Deactivate(updatedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> RemoveMemberAsync(Guid DepartmentId, Guid Id, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.RemoveMember(Id);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> SoftDeleteAsync(Guid DepartmentId, Guid deletedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.SoftDelete(deletedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> UpdateAsync(Guid DepartmentId, string title, string? description, Guid updatedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.UpdateDetails(title, description, updatedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }

        public async Task<Department> UpdateIconAsync(Guid DepartmentId, string icon, Guid updatedBy, CancellationToken cancellationToken)
        {
            var depart = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId, cancellationToken);
            if (depart == null) throw new KeyNotFoundException("Departement Not Found!");
            depart.UpdateIcon(icon, updatedBy);
            await dbContext.SaveChangesAsync(cancellationToken);
            return depart;
        }
    }
}