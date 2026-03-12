using System.ComponentModel.DataAnnotations;
namespace project_management_backend.Domain.Entities.Organization
{




    public class OrganizationMember
    {

        [Key]
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; } // <-- navigation property

        public Guid UserId { get; private set; }
        public User.User User { get; private set; }

        public OrganizationRole Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private OrganizationMember() { }

        public OrganizationMember(Guid userId, Guid organizationId, OrganizationRole role)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            OrganizationId = organizationId;
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