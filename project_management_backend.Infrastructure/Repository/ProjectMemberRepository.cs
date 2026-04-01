using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public ProjectMemberRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProjectMember> ChangeRole(Guid projectMemberId, Guid ProjectId, ProjectRole role, CancellationToken cancellationToken)
        {
            var projectMember = await dbContext.ProjectMembers.Include(m => m.User).FirstOrDefaultAsync(p => p.Id == projectMemberId && p.ProjectId == ProjectId, cancellationToken);
            if (projectMember == null) throw new NotImplementedException("Project Member not found");

            projectMember.ChangeRole(role);
            await dbContext.SaveChangesAsync(cancellationToken);

            return projectMember;
        }

        public async Task<ProjectMember> CreateProjectMember(Guid ProjectId, Guid UserId, ProjectRole role, CancellationToken cancellationToken)
        {
            var organizationMember = await dbContext.OrganizationMembers.FirstOrDefaultAsync(om => om.UserId == UserId, cancellationToken);
            if (organizationMember == null) throw new KeyNotFoundException("Organization Member do not exist");
            var ProjectMember = new ProjectMember(ProjectId, organizationMember.OrganizationId, UserId, role);

            await dbContext.ProjectMembers.AddAsync(ProjectMember, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return ProjectMember;
        }

        public async Task<ProjectMember> RemoveProjectMember(Guid projectMemberId, CancellationToken cancellationToken)
        {
            var projectMember = await dbContext.ProjectMembers.FirstOrDefaultAsync(pm => pm.Id == projectMemberId);
            if (projectMember == null) throw new NotImplementedException("Project Member Not Found");

            dbContext.ProjectMembers.Remove(projectMember);
            await dbContext.SaveChangesAsync();

            return projectMember;
        }
    }
}