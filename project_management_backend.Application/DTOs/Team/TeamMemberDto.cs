namespace project_management_backend.Application.Dto.Team
{
    public class TeamMemberResponseDto
    {
        public Guid OrganizationMemberId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}