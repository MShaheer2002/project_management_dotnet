using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.Application.Interface
{
    public interface IProjectMemberRepository
    {
        Task<ProjectMember> CreateProjectMember(Guid ProjectId, Guid UserId, ProjectRole role, CancellationToken cancellationToken);
        Task<ProjectMember> RemoveProjectMember(Guid projectMemberId, CancellationToken cancellationToken);
        Task<ProjectMember> ChangeRole(Guid projectMemberId, Guid ProjectId, ProjectRole role,CancellationToken cancellationToken);

    }
}