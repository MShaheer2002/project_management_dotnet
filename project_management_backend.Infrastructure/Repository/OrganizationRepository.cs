using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        public Task<Organization> CreateOrganizationAsync(Organization organization)
        {
            throw new NotImplementedException();

        }

        public Task<Organization?> DeleteOrganizationAsync(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Organization?>> GetAllOrganizationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Organization> GetOrganizationAsync(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Organization> UpdateAllOrganizationAsync()
        {
            throw new NotImplementedException();
        }
    }
}