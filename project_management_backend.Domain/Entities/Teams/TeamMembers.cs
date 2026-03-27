using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.Domain.Entities.TeamMembers
{
    public class TeamMember
    {
        public Guid Id { get; private set; }

        public Guid TeamId { get; private set; }
        public Team Team { get; private set; }

        public Guid OrganizationMemberId { get; private set; }
        public OrganizationMember OrganizationMember { get; private set; }

        public TeamRole Role { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private TeamMember() { } // EF

        public TeamMember(Guid teamId, Guid organizationMemberId, TeamRole role)
        {
            Id = Guid.NewGuid();
            TeamId = teamId;
            OrganizationMemberId = organizationMemberId;
            Role = role;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRole(TeamRole role)
        {
            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public enum TeamRole
    {
        Admin,
        Manager,

        Lead,
        Member
    }
}