using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.Application.Dto.Team
{
    public class CreateTeamRequestDto
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<CreateTeamMemberDto> Members { get; set; } = new();

    }

    public class CreateTeamResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public TeamStatus Status { get; set; }
    }

    public class CreateTeamMemberDto
    {
        public Guid OrganizationMemberId { get; set; }
        public TeamRole Role { get; set; }
    }
}