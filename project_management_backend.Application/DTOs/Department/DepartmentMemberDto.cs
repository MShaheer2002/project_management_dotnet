using project_management_backend.Application.Dto.user;
using project_management_backend.Domain.Entities.Department;

namespace project_management_backend.Application.Dto.Department
{
    public class DepartmentMemberResponseDto
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public GetUserResponseDto User { get; set; }
        public Guid WorkspaceMemberId { get; set; }

        public DepartmentRole Role { get; set; }

        public Guid AddedBy { get; set; }
        public DateTime JoinedAt { get; set; }

        public bool IsActive { get; set; }
        public bool IsRemoved { get; set; }

        public Guid? RemovedBy { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}