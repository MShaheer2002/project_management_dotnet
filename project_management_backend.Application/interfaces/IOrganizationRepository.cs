using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Application.Interface
{
    public interface IOrganizationRepository
    {
        Task<Organization> GetByIdAsync(Guid organizationId);
        Task<Organization> GetBySlugAsync(Guid organizationId);
        Task<List<Organization>> GetUserOrganizationsAsync(Guid userId);
        Task<Organization> CreateAsync(Organization organization);
        Task<Organization> UpdateAsync(Organization organization);
        Task<Organization?> DeleteAsync(Guid organizationId);
        Task<List<Organization>> GetAllAsync();
        Task AddMemberAsync(OrganizationMember member);
        Task RemoveMemberAsync(Guid organizationId, Guid userId);
        Task<bool> IsUserMemberAsync(Guid organizationId, Guid userId);
        Task<bool> IsSlugAvaibleAsync(string slug);

    }
}