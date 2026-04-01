using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.Application.Dto.Project
{
    public class ResponseProjectDto
    {
        public Guid Id { get; set; }
        public Guid ProjectOrganizationId { get; set; }
        public Guid WorkspaceId { get; set; }
        required public string Name { get; set; }
        required public string Key { get; set; }
        required public string Description { get; set; }
        required public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        required public string Visibility { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProjectMemberResponseDto>? projectMembers { get; set; }
    }


}