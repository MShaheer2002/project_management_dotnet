using project_management_backend.Domain.Entities.TeamMembers;

namespace project_management_backend.Application.Dto.Team
{

    public class TeamMemberRequestDto
    {
        public Guid OrganizationMemberId { get; set; }
        public TeamRole Role { get; set; }
    }

    public class TeamMemberResponseDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Name {get;set;}
        public string UserName { get; set; }
        public string? avatarUrl {get;set;}
        public string Email { get; set; }

    }
}