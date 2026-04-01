using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Workspace;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class WorkspaceMemberRepository : IWorkspaceMemberRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public WorkspaceMemberRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<WorkspaceMember> CreateWorkspace(WorkspaceMember workspaceMember)
        {
            await dbContext.WorkspaceMembers.AddAsync(workspaceMember);
            await dbContext.SaveChangesAsync();

            return workspaceMember;
        }
    }
}