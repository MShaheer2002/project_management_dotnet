using project_management_backend.Application.Dto.user;
using project_management_backend.Domain.Entities.Organizations;

namespace project_management_backend.Application.Dto.Organization
{
    public class GetOrganizationMemberResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public OrganizationRole Role { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime InvitedAt { get; set; }
        public DateTime? JoinedAt { get; set; }
        public GetUserResponseDto? User { get; set; }
    }
}
