using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.TeamMembers;

namespace project_management_backend.Domain.Entities.Teams
{
    public class Team
    {
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; }

        public Guid WorkspaceId { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public TeamStatus Status { get; private set; }

        // Navigation
        public Organization Organization { get; private set; }
        private readonly List<TeamMember> _members = new();

        public IReadOnlyCollection<TeamMember> Members => _members;

        public TeamMember AddMember(Guid organizationMemberId, TeamRole role)
        {
            if (_members.Any(x => x.OrganizationMemberId == organizationMemberId))
                throw new InvalidOperationException("User already in team");
            var member = new TeamMember(Id, organizationMemberId, role);
            _members.Add(member);
            return member;
        }

        private Team() { } // EF

        public Team(Guid organizationId, string name, Guid workspaceId, Guid createdByUserId)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            Name = name;
            CreatedByUserId = createdByUserId;
            WorkspaceId = workspaceId;
            Status = TeamStatus.Active;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string? description)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TeamStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            Status = TeamStatus.Deactived;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public enum TeamStatus
    {
        Active,
        Archived,
        Deactived
    }
}