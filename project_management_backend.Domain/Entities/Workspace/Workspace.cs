namespace project_management_backend.Domain.Entities.Workspace
{
    public class Workspace
    {
        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; }

        public WorkspaceVisibility Visibility { get; private set; }

        public Guid CreatedBy { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation
        public ICollection<WorkspaceMember> Members { get; private set; } = new List<WorkspaceMember>();

        private Workspace() { } // EF Core

        public Workspace(Guid organizationId, string name, Guid createdBy)
        {
            if (organizationId == Guid.Empty)
                throw new ArgumentException("OrganizationId cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Workspace must have a name");

            if (createdBy == Guid.Empty)
                throw new ArgumentException("CreatedBy cannot be empty");

            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            Name = name.Trim();
            Visibility = WorkspaceVisibility.Private;

            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Workspace name cannot be empty");

            Name = name.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeVisibility(WorkspaceVisibility visibility)
        {
            Visibility = visibility;
            UpdatedAt = DateTime.UtcNow;
        }
    }


    public enum WorkspaceVisibility
    {
        Private,        // Only members can access
        Organization    // All org users can view
    }


}