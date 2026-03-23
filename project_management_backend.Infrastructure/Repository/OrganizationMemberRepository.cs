using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class OrganizationMemberRepository : IOrganizationMemberRepository
    {
        private readonly ProjectManagementDbContext dbContext;
        private readonly IEmailService emailService;
        private readonly ILogger<OrganizationMemberRepository> logger;

        public OrganizationMemberRepository(ProjectManagementDbContext dbContext, IEmailService emailService, ILogger<OrganizationMemberRepository> logger)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
            this.logger = logger;
        }
        public async Task<OrganizationMember> AcceptInviteAsync(string token, Guid userId, string userEmail)
        {
            var member = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(m => m.InviteToken == token && !m.IsAccepted);
            // Log the token safely
            logger.LogInformation($"[AcceptInvite] Attempting to accept invite with token: {token}");

            if (member == null)
            {
                logger.LogWarning($"[AcceptInvite] No pending invite found for token: {token}");
                throw new InvalidOperationException("Invalid or expired invite.");
            }

            logger.LogInformation($"[AcceptInvite] Found invite for Email: {member.Email}, OrgId: {member.OrganizationId}");
            logger.LogInformation($"[AcceptInvite] Email in api: {userEmail}, email in db: {member.Email}");

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
        public async Task<OrganizationMember> ChangeRoleAsync(Guid memberId, OrganizationRole newRole, Guid currentUserId)
        {
            var orgMember = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(o => o.Id == memberId);

            if (orgMember == null)
                throw new InvalidOperationException("Organization Member not found!");

            var currentUser = await dbContext.OrganizationMembers
                .FirstOrDefaultAsync(o =>
                    o.UserId == currentUserId &&
                    o.OrganizationId == orgMember.OrganizationId);

            if (currentUser == null)
                throw new UnauthorizedAccessException("You are not part of this organization.");

            // Example rule: only Owner/Admin can change roles
            if (currentUser.Role != OrganizationRole.Owner &&
                currentUser.Role != OrganizationRole.Admin)
            {
                throw new UnauthorizedAccessException("You don't have permission.");
            }

            // Optional: prevent changing Owner
            if (orgMember.Role == OrganizationRole.Owner)
                throw new InvalidOperationException("Cannot change Owner role.");

            orgMember.ChangeRole(role: newRole);

            await dbContext.SaveChangesAsync();

            return orgMember;
        }
        public async Task<OrganizationMember> InviteMemberAsync(Guid orgId, string email, OrganizationRole role)
        {
            var isMemberAvaible = await dbContext.OrganizationMembers.AnyAsync(m => m.OrganizationId == orgId && m.Email == email);
            if (isMemberAvaible) throw new InvalidOperationException("User already invited or a member.");

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var member = new OrganizationMember(email, orgId, role, token);

            dbContext.OrganizationMembers.Add(member);
            await dbContext.SaveChangesAsync();

            var inviteLink = $"http://localhost:5297/api/organization/accept-invite?token={WebUtility.UrlEncode(token)}";
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