using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Application.Interface
{
    public interface IOrganizationMemberRepository
    {
        Task<OrganizationMember> InviteMemberAsync(Guid orgId, string email, OrganizationRole role);
        Task<OrganizationMember> AcceptInviteAsync(string token, Guid userId, string userEmail);
        Task<OrganizationMember> AddMemberDirectAsync(Guid orgId, Guid userId, OrganizationRole role);
        Task ChangeRoleAsync(Guid memberId, OrganizationRole newRole, Guid currentUserId);
    }
}