using project_management_backend.Domain.Entities.Users;

namespace project_management_backend.Domain.Entities.Project
{
    public class ProjectMember
    {
        public Guid Id { get; private set; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public Guid OrganizationMemberId { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public ProjectRole Role { get; private set; }

        public DateTime JoinedAt { get; private set; }

        private ProjectMember() { } // For EF Core

        public ProjectMember(Guid projectId, Guid organizationMemberId,Guid userId ,ProjectRole role)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId cannot be empty");

            if (organizationMemberId == Guid.Empty)
                throw new ArgumentException("OrganizationMemberId cannot be empty");

            Id = Guid.NewGuid();
            ProjectId = projectId;
            UserId = userId;
            OrganizationMemberId = organizationMemberId;
            Role = role;
            JoinedAt = DateTime.UtcNow;
        }

        public void ChangeRole(ProjectRole newRole)
        {
            if (Role == newRole)
                return;

            Role = newRole;
        }
    }

    public enum ProjectRole
    {
        Admin,
        Manager,
        Member,
        Viewer
    }
}