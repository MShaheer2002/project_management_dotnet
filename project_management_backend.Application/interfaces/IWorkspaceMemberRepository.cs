using project_management_backend.Domain.Entities.Workspace;

namespace project_management_backend.Application.Interface
{
    public interface IWorkspaceMemberRepository
    {
      Task<WorkspaceMember> CreateWorkspace(WorkspaceMember workspaceMember);
    }
}