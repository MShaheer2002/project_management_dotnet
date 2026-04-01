using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.Project;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Project;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository projectRepository;
        private readonly IProjectMemberRepository projectMemberRepository;

        public ProjectController(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository)
        {
            this.projectRepository = projectRepository;
            this.projectMemberRepository = projectMemberRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectRequestDto project, CancellationToken cancellationToken)
        {
            if (project == null) return NotFound(new ApiResponse<Object>(false, "", "Project Not Found"));

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

            var proj = new Project(project.ProjectOrganizationId, project.WorkspaceId, project.Key, project.Name, project.Description, userId, project.TargetDate);

            var ResProject = await projectRepository.CreateAsync(proj, cancellationToken);

            await projectMemberRepository.CreateProjectMember(ResProject.Id, userId, ProjectRole.Admin, cancellationToken);

            var ResponseProject = new CreateProjectResponseDto
            {
                Id = ResProject.Id,
                ProjectOrganizationId = ResProject.OrganizationId,
                WorkspaceId = ResProject.WorkspaceId,
                Name = ResProject.Name,
                Key = ResProject.Key,
                Description = ResProject.Description,
                Status = ResProject.Status.ToString(),
                CreatedBy = ResProject.CreatedBy,
                Visibility = ResProject.Visibility.ToString(),
                StartDate = ResProject.StartDate,
                TargetDate = ResProject.TargetDate,
                UpdatedAt = ResProject.UpdatedAt,
            };
            return Ok(new ApiResponse<CreateProjectResponseDto>(true, ResponseProject, "Succesfully Created!"));

        }
        [HttpGet("organizations/{OrganizationId}/projects")]
        public async Task<IActionResult> GetProjectsByOrganization(Guid OrganizationId, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10)
        {

            var projects = await projectRepository.GetByOrganizationAsync(OrganizationId, pageNumber, pageSize, cancellationToken);

            var projectsList = projects.Select(p =>
              new ResponseProjectDto
              {
                  Id = p.Id,
                  ProjectOrganizationId = p.OrganizationId,
                  WorkspaceId = p.WorkspaceId,
                  Name = p.Name,
                  Key = p.Key,
                  Description = p.Description,
                  Status = p.Status.ToString(),
                  CreatedBy = p.CreatedBy,
                  Visibility = p.Visibility.ToString(),
                  TargetDate = p.TargetDate,
                  UpdatedAt = p.UpdatedAt,
                  projectMembers = p.Members.Select(m => new ProjectMemberResponseDto
                  {
                      Id = m.Id,
                      ProjectId = m.ProjectId,
                      OrganizationMemberId = m.OrganizationMemberId,
                      User = new GetUserResponseDto
                      {
                          Id = m.User.Id,
                          FirstName = m.User.FirstName,
                          LastName = m.User.LastName,
                          UserName = m.User.UserName,
                          AvatarUrl = m.User.AvatarUrl
                      },
                      Role = m.Role.ToString(),
                      JoinedAt = m.JoinedAt
                  }).ToList()
              }
            ).ToList();
            return Ok(new ApiResponse<List<ResponseProjectDto>>(true, projectsList, "Success"));
        }

        [HttpPut("{ProjectId}/Update-Project")]
        public async Task<IActionResult> UpdateProject(string name, string description, DateTime targetDate, Guid ProjectId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");
            await projectRepository.UpdateAsync(ProjectId, name, description, targetDate, cancellationToken);

            return Ok(new ApiResponse<object>(true, null, "Successful"));
        }

        [HttpPost("Active-Project/{ProjectId}")]
        public async Task<IActionResult> Active(Guid ProjectId, CancellationToken cancellationToken)
        {
            await projectRepository.ActivateAsync(ProjectId, cancellationToken);
            return Ok(new ApiResponse<Object>(true, null, "Successful"));
        }

        [HttpPost("Archive-Project/{ProjectId}")]
        public async Task<IActionResult> Archive(Guid ProjectId, CancellationToken cancellationToken)
        {
            var archived = await projectRepository.ArchiveAsync(ProjectId, cancellationToken);
            if (!archived) return BadRequest(new ApiResponse<Object>(false, archived, "Something went wrong!"));
            return Ok(new ApiResponse<Object>(true, archived, "Successful"));
        }

        [HttpDelete("{ProjectId}")]
        public async Task<IActionResult> Delete(Guid ProjectId, CancellationToken cancellationToken)
        {
            await projectRepository.DeleteAsync(ProjectId, cancellationToken);
            return Ok(new ApiResponse<Object>(true, null, "Deleted"));
        }

        [HttpGet("Project-Exists/{ProjectId}")]
        public async Task<IActionResult> ProjectExists(Guid ProjectId, CancellationToken cancellationToken)
        {
            var doesExist = await projectRepository.ExistsAsync(ProjectId, cancellationToken);
            if (!doesExist) return BadRequest(new ApiResponse<Object>(false, doesExist, "Something went wrong!"));
            return Ok(new ApiResponse<Object>(true, doesExist, "Successful"));

        }

        [HttpGet("Get-By-Id/{ProjectId}")]
        public async Task<IActionResult> GetById(Guid ProjectId, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(ProjectId, cancellationToken);
            if (project == null) return BadRequest(new ApiResponse<Object>(false, null, "Project Not Found!"));

            var Project =
                  new ResponseProjectDto
                  {
                      Id = project.Id,
                      ProjectOrganizationId = project.OrganizationId,
                      WorkspaceId = project.WorkspaceId,
                      Name = project.Name,
                      Key = project.Key,
                      Description = project.Description,
                      Status = project.Status.ToString(),
                      CreatedBy = project.CreatedBy,
                      Visibility = project.Visibility.ToString(),
                      TargetDate = project.TargetDate,
                      UpdatedAt = project.UpdatedAt,
                      projectMembers = project.Members.Select(m => new ProjectMemberResponseDto
                      {
                          Id = m.Id,
                          ProjectId = m.ProjectId,
                          OrganizationMemberId = m.OrganizationMemberId,
                          User = new GetUserResponseDto
                          {
                              Id = m.User.Id,
                              FirstName = m.User.FirstName,
                              LastName = m.User.LastName,
                              UserName = m.User.UserName,
                              AvatarUrl = m.User.AvatarUrl
                          },
                          Role = m.Role.ToString(),
                          JoinedAt = m.JoinedAt
                      }).ToList()
                  };
            return Ok(new ApiResponse<Object>(true, Project, "Successful"));

        }

        [HttpGet("Get-By-Key/{OrganizationId}")]
        public async Task<IActionResult> GetByKey(string key, Guid OrganizationId, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByKeyAsync(key, OrganizationId, cancellationToken);
            if (project == null) return BadRequest(new ApiResponse<Object>(false, null, "Project Not Found!"));

            var Project =
                  new ResponseProjectDto
                  {
                      Id = project.Id,
                      ProjectOrganizationId = project.OrganizationId,
                      WorkspaceId = project.WorkspaceId,
                      Name = project.Name,
                      Key = project.Key,
                      Description = project.Description,
                      Status = project.Status.ToString(),
                      CreatedBy = project.CreatedBy,
                      Visibility = project.Visibility.ToString(),
                      TargetDate = project.TargetDate,
                      UpdatedAt = project.UpdatedAt,
                      projectMembers = project.Members.Select(m => new ProjectMemberResponseDto
                      {
                          Id = m.Id,
                          ProjectId = m.ProjectId,
                          OrganizationMemberId = m.OrganizationMemberId,
                          User = new GetUserResponseDto
                          {
                              Id = m.User.Id,
                              FirstName = m.User.FirstName,
                              LastName = m.User.LastName,
                              UserName = m.User.UserName,
                              AvatarUrl = m.User.AvatarUrl
                          },
                          Role = m.Role.ToString(),
                          JoinedAt = m.JoinedAt
                      }).ToList()
                  };
            return Ok(new ApiResponse<Object>(true, Project, "Successful"));

        }

        [HttpPost("{ProjectId}/Change-ProjectMember-Role/{ProjectMemberId}")]
        public async Task<ActionResult> ChangeRole(Guid ProjectId, Guid ProjectMemberId, ProjectRole role, CancellationToken cancellationToken)
        {
            var projectMember = await projectMemberRepository.ChangeRole(ProjectMemberId, ProjectId, role, cancellationToken);
            var ProjectMember = new ProjectMemberResponseDto
            {
                Id = projectMember.Id,
                ProjectId = projectMember.ProjectId,
                OrganizationMemberId = projectMember.OrganizationMemberId,
                User = new GetUserResponseDto
                {
                    Id = projectMember.User.Id,
                    FirstName = projectMember.User.FirstName,
                    LastName = projectMember.User.LastName,
                    UserName = projectMember.User.UserName,
                    AvatarUrl = projectMember.User.AvatarUrl
                },

                Role = projectMember.Role.ToString(),
                JoinedAt = projectMember.JoinedAt,
            };
            return Ok(new ApiResponse<Object>(true, ProjectMember, "Successful"));
        }

        [HttpPost("Remove-ProjectMember/{ProjectMemberId}")]
        public async Task<ActionResult> ChangeRole(Guid ProjectMemberId, CancellationToken cancellationToken)
        {
            var projectMember = await projectMemberRepository.RemoveProjectMember(ProjectMemberId, cancellationToken);
            var ProjectMember = new ProjectMemberResponseDto
            {
                Id = projectMember.Id,
                ProjectId = projectMember.ProjectId,
                OrganizationMemberId = projectMember.OrganizationMemberId,
                User = new GetUserResponseDto
                {
                    Id = projectMember.User.Id,
                    FirstName = projectMember.User.FirstName,
                    LastName = projectMember.User.LastName,
                    UserName = projectMember.User.UserName,
                    AvatarUrl = projectMember.User.AvatarUrl
                },

                Role = projectMember.Role.ToString(),
                JoinedAt = projectMember.JoinedAt,
            };
            return Ok(new ApiResponse<Object>(true, ProjectMember, "Successful"));
        }
    }
}