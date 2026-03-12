using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Application.Interface
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task<Organization> GetOrganizationAsync(Guid organizationId);
        Task<Organization?> DeleteOrganizationAsync(Guid organizationId);
        Task<List<Organization?>> GetAllOrganizationsAsync();
        Task<Organization> UpdateAllOrganizationAsync();

        Task<bool> IsSlugAvaibleAsync(string slug);

    }
}