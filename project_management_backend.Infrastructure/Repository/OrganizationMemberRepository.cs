using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class OrganizationMemberRepository : IOrganizationMemberRepository
    {
        private readonly ProjectManagementDbContext dbContext;
        private readonly IEmailService emailService;

        public OrganizationMemberRepository(ProjectManagementDbContext dbContext, IEmailService emailService)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
        }
        public async Task<OrganizationMember> AcceptInviteAsync(string token, Guid userId, string userEmail)
        {
            var member = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(m => m.InviteToken == token && !m.IsAccepted);

            if (member == null)
                throw new InvalidOperationException("Invalid or expired invite.");

            if (member.Email != userEmail)
                throw new UnauthorizedAccessException("Invite does not belong to this user.");

            member.AcceptInvite(userId);
            await dbContext.SaveChangesAsync();

            return member;

        }


        public Task<OrganizationMember> AddMemberDirectAsync(Guid orgId, Guid userId, OrganizationRole role)
        {
            throw new NotImplementedException();
        }

        public Task ChangeRoleAsync(Guid memberId, OrganizationRole newRole, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrganizationMember> InviteMemberAsync(Guid orgId, string email, OrganizationRole role)
        {
            var isMemberAvaible = await dbContext.OrganizationMembers.AnyAsync(m => m.OrganizationId == orgId && m.Email == email);
            if (isMemberAvaible) throw new InvalidOperationException("User already invited or a member.");

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var member = new OrganizationMember(email, orgId, role, token);

            dbContext.OrganizationMembers.Add(member);
            await dbContext.SaveChangesAsync();

            var inviteLink = "http://localhost:5297/api/organization/accept-invite?token={token}";
            var body = $@"
                <h3>You're invited</h3>
                <p>Click below to join:</p>
                <a href='{inviteLink}'>Accept Invite</a>
                ";
            await emailService.SendEmailAsync(email, "TaskEnv Invitation", body);
            return member;

        }
    }
}