namespace project_management_backend.Domain.Entities.Workspace
{
    public class WorkspaceMember
    {
        public Guid Id { get; private set; }

        public Guid WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }

        public Guid OrganizationMemberId { get; private set; }

        public WorkspaceRole Role { get; private set; }

        public DateTime JoinedAt { get; private set; }

        private WorkspaceMember() { }

        public WorkspaceMember(Guid workspaceId, Guid organizationMemberId, WorkspaceRole role)
        {
            if (workspaceId == Guid.Empty)
                throw new ArgumentException("WorkspaceId cannot be empty");

            if (organizationMemberId == Guid.Empty)
                throw new ArgumentException("OrganizationMemberId cannot be empty");

            Id = Guid.NewGuid();
            WorkspaceId = workspaceId;
            OrganizationMemberId = organizationMemberId;
            Role = role;
            JoinedAt = DateTime.UtcNow;
        }

        public void ChangeRole(WorkspaceRole role)
        {
            Role = role;
        }
    }

    public enum WorkspaceRole
    {
        Admin,
        Member,
        Viewer
    }
}