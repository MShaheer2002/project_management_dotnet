namespace project_management_backend.Domain.Issues
{
    public class Issue
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; } = null!;
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;

        public Guid ProjectId { get; private set; }
        public Guid TeamId { get; private set; }
        public Guid WorkspaceId { get; private set; }

        public Priority Priority { get; private set; }

        public int? Estimate { get; private set; }

        public Guid? CycleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Issue() { } // use for EF
        public Issue(string key, string title, string? description, Priority? priority, Guid projectId, Guid teamId, Guid workspaceId)
        {
            Id = Guid.NewGuid();
            Key = string.IsNullOrWhiteSpace(key) ? throw new ArgumentException("Key cannot be null") : key;
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be null") : title;
            Description = description ?? "";
            ProjectId = projectId;
            Priority = priority ?? Priority.low;
            TeamId = teamId;
            WorkspaceId = workspaceId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

        }
        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty");

            Title = newTitle;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription ?? "";
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePriority(Priority priority)
        {
            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
        }

    }
    public enum Priority { low, medium, high, urgent }

}