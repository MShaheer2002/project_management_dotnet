using project_management_backend.Domain.Entities.Organizations;
using project_management_backend.Domain.Entities.TeamMembers;

namespace project_management_backend.Domain.Entities.Teams
{
    public class Team
    {
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; }
        public string? Description { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public TeamStatus Status { get; private set; }

        // Navigation
        public Organization Organization { get; private set; }
        private readonly List<TeamMember> _members = new();

        public IReadOnlyCollection<TeamMember> Members => _members;

        public void AddMember(Guid organizationMemberId, TeamRole role)
        {
            if (_members.Any(x => x.OrganizationMemberId == organizationMemberId))
                throw new InvalidOperationException("User already in team");

            _members.Add(new TeamMember(Id, organizationMemberId, role));
        }

        private Team() { } // EF

        public Team(Guid organizationId, string name, Guid createdByUserId, string? description = null)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            CreatedByUserId = createdByUserId;

            Status = TeamStatus.Active;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            Status = TeamStatus.Archived;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public enum TeamStatus
    {
        Active,
        Archived
    }
}