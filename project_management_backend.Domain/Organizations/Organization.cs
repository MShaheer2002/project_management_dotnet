using System.Data.Common;

namespace project_management_backend.Domain.Organization
{
    public class Organization
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public string? Slug { get; private set; }

        public Guid OwnerUserId { get; private set; }
        public OrganizationStatus Status { get; private set; }
        public DateTime? TrialEndAt { get; private set; }
        private readonly List<OrganizationMember> _members = new();
        public IReadOnlyCollection<OrganizationMember> Members => _members.AsReadOnly();


        public DateTime? CreatedAt { get; private set; }
            public DateTime? UpdatedAt { get; private set; }

        private Organization() { } // for EF
        public Organization(string name, string slug, Guid ownerUserId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Organization must have a name.");

            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Organization must have a slug.");

            Id = Guid.NewGuid();
            Name = name;
            Slug = slug.ToLowerInvariant();
            OwnerUserId = ownerUserId;
            CreatedAt = DateTime.UtcNow;

            _members.Add(new OrganizationMember(ownerUserId, OrganizationRole.Owner));
        }

        public void AddMember(Guid userId, OrganizationRole role)
        {
            if (_members.Any(x => x.UserId == userId))
                throw new InvalidOperationException("User already a member.");

            _members.Add(new OrganizationMember(userId, role));
        }

        public void RemoveMember(Guid userId)
        {
            var member = _members.FirstOrDefault(x => x.UserId == userId);
            if (member == null)
                throw new InvalidOperationException("Member not found.");

            if (member.Role == OrganizationRole.Owner)
                throw new InvalidOperationException("Cannot remove owner.");

            _members.Remove(member);
        }

        public void Suspend()
        {
            Status = OrganizationStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = OrganizationStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }


    }

    public enum OrganizationStatus
    {
        Active,
        Suspended,


    }
}