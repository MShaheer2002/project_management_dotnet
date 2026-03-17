using System.ComponentModel.DataAnnotations;
using project_management_backend.Domain.Entities.Users;

namespace project_management_backend.Domain.Entities.Organization
{
    public class OrganizationMember
    {
        [Key]
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; }

        public Guid? UserId { get; private set; } // Nullable if user hasn't accepted invite yet
        public User User { get; private set; }
        public string Email { get; private set; } // For invite
        public string InviteToken { get; private set; } // Verification token
        public bool IsAccepted { get; private set; } = false;
        public DateTime InvitedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? JoinedAt { get; private set; } // When user accepts
        public OrganizationRole Role { get; private set; }

        private OrganizationMember() { }

        // Constructor for direct add (existing user)
        public OrganizationMember(Guid userId, Guid organizationId, OrganizationRole role)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            OrganizationId = organizationId;
            Role = role;
            JoinedAt = DateTime.UtcNow;
            IsAccepted = true;
        }
        
        // Constructor for invite (user not yet in system)
        public OrganizationMember(string email, Guid organizationId, OrganizationRole role, string inviteToken)
        {
            Id = Guid.NewGuid();
            Email = email;
            OrganizationId = organizationId;
            Role = role;
            InviteToken = inviteToken;
            InvitedAt = DateTime.UtcNow;
            IsAccepted = false;
        }

        public void AcceptInvite(Guid userId)
        {
            UserId = userId;
            JoinedAt = DateTime.UtcNow;
            InviteToken = null;
            IsAccepted = true;
        }

        public void ChangeRole(OrganizationRole role)
        {
            Role = role;
        }
    }
    public enum OrganizationRole
    {
        Owner = 0,
        Admin = 1,
        Member = 2,
        Guest = 3
    }
}