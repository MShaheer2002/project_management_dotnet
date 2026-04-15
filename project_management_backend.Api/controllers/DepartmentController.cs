using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.Department;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Department;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IDepartmentMemberRepository departmentMemberRepository;
        private readonly ILogger<DepartmentController> logger;
        public DepartmentController(IDepartmentRepository departmentRepository, IDepartmentMemberRepository departmentMemberRepository, ILogger<DepartmentController> logger)
        {
            this.logger = logger;
            this.departmentMemberRepository = departmentMemberRepository;
            this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(string title, string? description, Guid orgId, Guid workspaceId, string? icon, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userEmail = User.FindFirstValue(ClaimTypes.Email);
            // if (userIdClaim == null || userEmail == null) return Unauthorized();
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);
            var depart = await departmentRepository.CreateAsync(title, description, orgId, workspaceId, userId, icon, cancellationToken);

            var Department = new DepartmentResponseDto
            {
                Id = depart.Id,
                Title = depart.Title,
                Slug = depart.Slug,
                Description = depart.Description,
                OrganizationId = depart.OrganizationId,
                WorkspaceId = depart.WorkspaceId,
                LeaderId = depart.LeaderId,
                IsActive = depart.IsActive,
                IsDeleted = depart.IsDeleted,
                Icon = depart.Icon,
                CreatedBy = depart.CreatedBy,
                CreatedAt = depart.CreatedAt,
                UpdatedBy = depart.UpdatedBy,
                UpdatedAt = depart.UpdatedAt,
                MemberCount = depart.Members.Count,
                DepartmentMembers = depart.Members.Select(d =>
                    new DepartmentMemberResponseDto
                    {
                        Id = d.Id,
                        DepartmentId = d.DepartmentId,
                        User = new GetUserResponseDto
                        {
                            Id = d.User.Id,
                            FirstName = d.User.FirstName,
                            LastName = d.User.LastName,
                            UserName = d.User.UserName,
                            AvatarUrl = d.User.AvatarUrl
                        },
                        WorkspaceMemberId = d.WorkspaceMemberId,
                        Role = d.Role,
                        AddedBy = d.AddedBy,
                        JoinedAt = d.JoinedAt,
                        IsActive = d.IsActive,
                        IsRemoved = d.IsRemoved,
                        RemovedBy = d.RemovedBy,
                        RemovedAt = d.RemovedAt,
                        UpdatedBy = d.UpdatedBy,
                    }
                ).ToList(),
            };

            return Ok(new ApiResponse<DepartmentResponseDto>(true, Department, "Success"));

        }

        [HttpPost("Add-Member/{DepartmentId}/{UserId}/{WorkspaceMemberId}")]
        public async Task<IActionResult> AddMember(Guid UserId, Guid WorkspaceMemberId, Guid DepartmentId, DepartmentRole role, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var AddedBy = Guid.Parse(userIdClaim);
            var departMemb = new DepartmentMember(DepartmentId, UserId, WorkspaceMemberId, role, AddedBy);
            var depart = await departmentRepository.AddMemberAsync(AddedBy, DepartmentId, UserId, role, cancellationToken);

            var Department = new DepartmentResponseDto
            {
                Id = depart.Id,
                Title = depart.Title,
                Slug = depart.Slug,
                Description = depart.Description,
                OrganizationId = depart.OrganizationId,
                WorkspaceId = depart.WorkspaceId,
                LeaderId = depart.LeaderId,
                IsActive = depart.IsActive,
                IsDeleted = depart.IsDeleted,
                Icon = depart.Icon,
                CreatedBy = depart.CreatedBy,
                CreatedAt = depart.CreatedAt,
                UpdatedBy = depart.UpdatedBy,
                UpdatedAt = depart.UpdatedAt,
                MemberCount = depart.Members.Count,
                DepartmentMembers = depart.Members.Select(d =>
                    new DepartmentMemberResponseDto
                    {
                        Id = d.Id,
                        DepartmentId = d.DepartmentId,
                        User = new GetUserResponseDto
                        {
                            Id = d.User.Id,
                            FirstName = d.User.FirstName,
                            LastName = d.User.LastName,
                            UserName = d.User.UserName,
                            AvatarUrl = d.User.AvatarUrl
                        },
                        Role = d.Role,
                        AddedBy = d.AddedBy,
                        JoinedAt = d.JoinedAt,
                        IsActive = d.IsActive,
                        IsRemoved = d.IsRemoved,
                        RemovedBy = d.RemovedBy,
                        RemovedAt = d.RemovedAt,
                        UpdatedBy = d.UpdatedBy,
                    }
             ).ToList(),
            };

            return Ok(new ApiResponse<DepartmentResponseDto>(true, Department, "Success"));
        }
    }
}