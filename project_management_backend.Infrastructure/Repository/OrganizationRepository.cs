using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;
using project_management_backend.Domain.Entities.Users;
using project_management_backend.Domain.Entities.Workspace;
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

        public async Task<Organization> CreateAsync(Organization organization, OrganizationMember organizationMember, Workspace workspace, WorkspaceMember workspaceMember, Team team, TeamMember teamMember, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await dbContext.Organizations.AddAsync(organization, cancellationToken);
                await dbContext.OrganizationMembers.AddAsync(organizationMember, cancellationToken);
                await dbContext.Workspaces.AddAsync(workspace, cancellationToken);
                await dbContext.WorkspaceMembers.AddAsync(workspaceMember, cancellationToken);
                await dbContext.Teams.AddAsync(team, cancellationToken);
                await dbContext.TeamMembers.AddAsync(teamMember, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return organization;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<Organization?> DeactiveAsync(Guid requesterId, Guid organizationId)
        {
            var org = await dbContext.Organizations
                .FirstOrDefaultAsync(o => o.Id == organizationId);

            if (org == null)
                throw new KeyNotFoundException("Organization not found");

            if (org.OwnerUserId != requesterId)
                throw new UnauthorizedAccessException("Not allowed to deactivate this organization");

            if (org.Status == OrganizationStatus.Deactivated)
                throw new InvalidOperationException("Already Deactived");

            org.ChangeStatus(OrganizationStatus.Deactivated);

            await dbContext.SaveChangesAsync();

            return org;
        }

        public async Task<List<Organization>> GetAllAsync()
        {
            return await dbContext.Organizations.Include(o => o.Owner).ToListAsync() ?? [];
        }

        public async Task<Organization?> GetByIdAsync(Guid organizationId)
        {
            return await dbContext.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);
        }

        public Task<Organization?> GetBySlugAsync(string slug)
        {
            return dbContext.Organizations.FirstOrDefaultAsync(o => o.Slug == slug);
        }

        public async Task<List<Organization>> GetUserOrganizationsAsync(Guid userId)
        {
            return await dbContext.OrganizationMembers
                .Where(m => m.UserId == userId)
                .Select(m => m.Organization)
                .ToListAsync();
        }

        public async Task<bool> IsSlugAvaibleAsync(string slug)
        {
            var isSlugAvaible = !await dbContext.Organizations.AnyAsync(r => r.Slug == slug);
            return isSlugAvaible;
        }

        public async Task<bool> IsUserMemberAsync(Guid organizationId, Guid userId)
        {
            return await dbContext.OrganizationMembers
                .AnyAsync(o =>
                    o.OrganizationId == organizationId &&
                    o.UserId == userId);
        }

        public async Task RemoveMemberAsync(Guid organizationId, Guid currentUserId, Guid targetUserId)
        {
            var org = await dbContext.Organizations
                .FirstOrDefaultAsync(o => o.Id == organizationId);

            if (org == null)
                throw new KeyNotFoundException("Organization not found");

            var requester = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(u =>
                    u.UserId == currentUserId &&
                    u.OrganizationId == organizationId);

            if (requester == null)
                throw new UnauthorizedAccessException("You are not part of this organization");

            if (requester.Role != OrganizationRole.Admin &&
                requester.Role != OrganizationRole.Owner)
            {
                throw new UnauthorizedAccessException("You are not authorized to remove members");
            }

            var target = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(o =>
                    o.UserId == targetUserId &&
                    o.OrganizationId == organizationId);

            if (target == null)
                throw new KeyNotFoundException("User to remove not found");

            if (target.Role == OrganizationRole.Owner)
                throw new UnauthorizedAccessException("Cannot remove the owner");

            if (target.UserId == requester.UserId)
                throw new InvalidOperationException("You cannot remove yourself");

            // Optional: prevent admin vs admin removal
            if (requester.Role == OrganizationRole.Admin &&
                target.Role == OrganizationRole.Admin)
            {
                throw new UnauthorizedAccessException("Admins cannot remove other admins");
            }

            dbContext.OrganizationMembers.Remove(target);

            await dbContext.SaveChangesAsync();
        }

        public Task<Organization> UpdateAsync(Organization organization)
        {
            throw new NotImplementedException();
        }
    }
}