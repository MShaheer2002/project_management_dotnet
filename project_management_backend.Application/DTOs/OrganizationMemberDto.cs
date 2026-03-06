
namespace project_management_backend.Domain.Organization
{
    public class OrganizationMemberDto
    {
        public Guid OrganizationId { get; set; }

        public Guid UserId { get; set; }
        public OrganizationRole role { get; set; }

        public DateTime joinedAt { get; set; }


    }

}

