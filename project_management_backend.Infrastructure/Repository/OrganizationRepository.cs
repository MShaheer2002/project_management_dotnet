using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Domain.Entities.User;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public OrganizationRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task AddMemberAsync(OrganizationMember member)
        {
            throw new NotImplementedException();
        }

        public async Task<Organization> CreateAsync(Organization organization)
        {
            await dbContext.Organizations.AddAsync(organization);
            await dbContext.SaveChangesAsync();

            return organization;

        }

        public Task<Organization?> DeleteAsync(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Organization?>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Organization> GetByIdAsync(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<Organization> GetBySlugAsync(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Organization>> GetUserOrganizationsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsSlugAvaibleAsync(string slug)
        {
            var isSlugAvaible = !await dbContext.Organizations.AnyAsync(r => r.Slug == slug);
            return isSlugAvaible;
        }

        public Task<bool> IsUserMemberAsync(Guid organizationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMemberAsync(Guid organizationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Organization> UpdateAsync(Organization organization)
        {
            throw new NotImplementedException();
        }
    }
}