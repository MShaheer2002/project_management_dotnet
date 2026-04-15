namespace project_management_backend.Application.Dto.Department
{
    public class DepartmentRequestDto
    {
        required public string Title { get; set; }
        public string? Description { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string? Icon { get; set; }
    }
    public class DepartmentResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid WorkspaceId { get; set; }

        public Guid? LeaderId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string? Icon { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int MemberCount { get; set; }

        public List<DepartmentMemberResponseDto>? DepartmentMembers { get; set; }

    }
}