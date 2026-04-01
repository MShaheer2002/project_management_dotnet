using project_management_backend.Application.Dto.user;

namespace project_management_backend.Application.Dto.Project
{
    public class ProjectMemberResponseDto
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid OrganizationMemberId { get; set; }

        public GetUserResponseDto User { get; set; }

        public string Role { get; set; }  

        public DateTime JoinedAt { get; set; }
    }
}