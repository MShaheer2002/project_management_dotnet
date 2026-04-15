using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;
using project_management_backend.Domain.Entities.Workspace;

namespace project_management_backend.Application.Interface
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateAsync(Organization organization, OrganizationMember organizationMember, Workspace workspace, WorkspaceMember workspaceMember,Team team, TeamMember teamMember, CancellationToken cancellationToken);
        Task<Organization?> GetByIdAsync(Guid organizationId);
        Task<Organization?> GetBySlugAsync(string slug);
        Task<List<Organization>> GetUserOrganizationsAsync(Guid userId);
        Task<Organization> UpdateAsync(Organization organization);
        Task<Organization?> DeactiveAsync(Guid requesterId, Guid organizationId);
        Task<List<Organization>> GetAllAsync();
        Task RemoveMemberAsync(Guid organizationId, Guid UserId, Guid RemoveUserId);
        Task<bool> IsUserMemberAsync(Guid organizationId, Guid userId);
        Task<bool> IsSlugAvaibleAsync(string slug);

    }
}