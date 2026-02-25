namespace project_management_backend.Domain.Project
{
    public class Project 
    {
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Guid WorkspaceId { get; private set; }
        public Guid TeamId { get; private set; }
        public string? Name { get; private set; }
        public string? Description { get; private set; }
        public ProjectStatus Status { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? TargetDate { get; private set; }
        public Guid CreateBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private Project() { }
        public Project(Guid organizationId, Guid workspaceId, Guid teamId, string name, string? description, Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Project must have a name");
            }

            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            WorkspaceId = workspaceId;
            TeamId = teamId;
            Name = name;
            Description = description ?? "";
            Status = ProjectStatus.Planned;
            CreateBy = createdBy;
        }

        public void UpdateDetails(string name, string? description, DateTime? startDate, DateTime? targetDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty.");

            Name = name;
            Description = description;
            StartDate = startDate;
            TargetDate = targetDate;
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
        Planned = 0,
        Active = 1,
        Completed = 2,
        Archived = 3
    }
}