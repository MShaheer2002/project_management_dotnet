namespace project_management_backend.Domain.Entities.Department
{
    public class Department
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Slug { get; private set; } = null!;
        public string? Description { get; private set; }

        public Guid OrganizationId { get; private set; }
        public Guid WorkspaceId { get; private set; }

        public Guid? LeaderId { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        public string? Icon { get; private set; }

        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Guid? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<DepartmentMember> _members = new();
        public IReadOnlyCollection<DepartmentMember> Members => _members.AsReadOnly();

        // EF Core
        private Department() { }

        public Department(
            string title,
            string? description,
            Guid organizationId,
            Guid workspaceId,
            Guid createdBy,
            string? icon = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Department title cannot be empty");

            Id = Guid.NewGuid();
            Title = title.Trim();
            Slug = GenerateSlug(title);

            Description = description?.Trim();

            OrganizationId = organizationId;
            WorkspaceId = workspaceId;

            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;

            Icon = icon;

            IsActive = true;
            IsDeleted = false;
        }


        public void UpdateDetails(string title, string? description, Guid updatedBy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Department title cannot be empty");

            Title = title.Trim();
            Slug = GenerateSlug(title);
            Description = description?.Trim();

            SetUpdated(updatedBy);
        }

        public void ChangeLeader(Guid? leaderId, Guid updatedBy)
        {
            LeaderId = leaderId;
            SetUpdated(updatedBy);
        }

        public void Activate(Guid updatedBy)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Cannot activate a deleted department");

            IsActive = true;
            SetUpdated(updatedBy);
        }

        public void Deactivate(Guid updatedBy)
        {
            IsActive = false;
            SetUpdated(updatedBy);
        }

        public void SoftDelete(Guid deletedBy)
        {
            if (IsDeleted) return;

            IsDeleted = true;
            IsActive = false;
            SetUpdated(deletedBy);
        }

        public void UpdateIcon(string? icon, Guid updatedBy)
        {
            Icon = icon;
            SetUpdated(updatedBy);
        }

        public void AddMember(DepartmentMember member)
        {
            if (_members.Any(m => m.UserId == member.UserId))
                throw new InvalidOperationException("User already exists in department");

            _members.Add(member);
        }

  
        public void RemoveMember(Guid userId)
        {
            var member = _members.FirstOrDefault(m => m.UserId == userId);
            if (member == null) return;

            _members.Remove(member);
        }

        //Helper
        private void SetUpdated(Guid userId)
        {
            UpdatedBy = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        private static string GenerateSlug(string value)
        {
            return value
                .Trim()
                .ToLower()
                .Replace(" ", "-"); // keep simple, improve later if needed
        }
    }
}