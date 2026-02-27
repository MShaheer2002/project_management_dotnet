namespace project_management_backend.Domain.Organization
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task<Organization> GetOrganizationAsync(Guid organizationId);
        Task<Organization?> DeleteOrganizationAsync(Guid organizationId);
        Task<List<Organization?>> GetAllOrganizationsAsync();
        Task<Organization> UpdateAllOrganizationAsync();


    }
}