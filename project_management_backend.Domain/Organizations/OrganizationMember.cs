namespace project_management_backend.Domain.Organization
{


    public class OrganizationMember
    {
        public Guid UserId { get; private set; }
        public OrganizationRole Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private OrganizationMember() { }

        public OrganizationMember(Guid userId, OrganizationRole role)
        {
            UserId = userId;
            Role = role;
            JoinedAt = DateTime.UtcNow;
        }

        public void ChangeRole(OrganizationRole role)
        {
            Role = role;
        }
    }


    public enum OrganizationRole
    {
        Owner = 0,
        Admin = 1,
        Member = 2
    }
}