namespace project_management_backend.Domain.Entities.Project
{
    public class Project
    {
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Guid WorkspaceId { get; private set; }
        public string Name { get; private set; }
        public string Key { get; private set; }
        public string Description { get; private set; }
        public ProjectStatus Status { get; private set; }
        public ProjectVisibility Visibility { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime TargetDate { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Navigation
        public ICollection<ProjectMember> Members { get; private set; } = new List<ProjectMember>();

        private Project() { }
        public Project(Guid organizationId, Guid workspaceId, string key, string name, string description, Guid createdBy, DateTime targetDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project must have a name");
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Project must have a key");


            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            WorkspaceId = workspaceId;
            Key = key.ToUpper();
            Name = name;
            Description = description ?? "";
            Status = ProjectStatus.Planned;
            Visibility = ProjectVisibility.Private;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            StartDate =  DateTime.UtcNow;
            TargetDate = targetDate;
            UpdatedAt =  DateTime.UtcNow;

        }

        public void UpdateDetails(string name, string description, DateTime targetDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty.");

            Name = name;
            Description = description ?? "";
            TargetDate = targetDate;
            UpdatedAt = DateTime.UtcNow;
        }
        public void ChangeVisibility(ProjectVisibility visibility)
        {
            Visibility = visibility;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = ProjectStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            Status = ProjectStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            Status = ProjectStatus.Archived;
            UpdatedAt = DateTime.UtcNow;
        }
    }
    public enum ProjectStatus
    {
        Planned,
        Active,
        Completed,
        Archived
    }

    public enum ProjectVisibility
    {
        Private,        // Only explicitly added project members can access
        Organization,   // Anyone in the organization can view (but not necessarily edit)
        Public         // Accessible outside the organization (use carefully)
    }
}