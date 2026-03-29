namespace project_management_backend.Application.Dto.Project
{
    public class CreateProjectRequestDto
    {
        public Guid ProjectOrganizationId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class CreateProjectResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProjectOrganizationId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        public string Visibility { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}