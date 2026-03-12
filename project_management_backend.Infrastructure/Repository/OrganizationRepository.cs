using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public  OrganizationRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public async Task<bool> IsSlugAvaibleAsync(string slug)
        {
            var isSlugAvaible = !await dbContext.Organizations.AnyAsync(r=>r.Slug == slug);
            return isSlugAvaible;
        }

        public Task<Organization> UpdateAllOrganizationAsync()
        {
            throw new NotImplementedException();
        }
    }
}