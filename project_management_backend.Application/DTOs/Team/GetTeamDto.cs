using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.Application.Dto.Team
{
    public class GetTeamResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }

        public List<TeamMemberResponseDto> TeamMembers { get; set; }
    }
}