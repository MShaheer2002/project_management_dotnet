using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.Team;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamRequestDto createTeamRequestDto)
        {
            var userIdCreate = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdCreate == null) return Unauthorized();
            var userId = Guid.Parse(userIdCreate);

            var createTeam = new Team(
                organizationId: createTeamRequestDto.OrganizationId,
                name: createTeamRequestDto.Name,
                createdByUserId: userId,
                description: createTeamRequestDto.Description ?? ""
            );

            foreach (var member in createTeamRequestDto.Members)
            {
                createTeam.AddMember(member.OrganizationMemberId, member.Role);
            }

            var created = await teamRepository.CreateTeamAsync(createTeam);

            var team = new CreateTeamResponseDto
            {
                Id = created.Id,
                OrganizationId = created.OrganizationId,
                Name = created.Name,
                Description = created.Description,
                CreatedByUserId = created.CreatedByUserId,
                Status = created.Status,
                CreatedAt = created.CreatedAt

            };

            return Ok(new ApiResponse<CreateTeamResponseDto>(true, team, "Successfully Created new Team"));

        }

        [HttpGet("Get-all-by-OrganizationId/{OrgId}")]
        public async Task<IActionResult> GetAllByOrgId(Guid OrgId)
        {
            var teams = await teamRepository.GetAllTeamByOrganizationAsync(OrgId);

            return Ok(new ApiResponse<List<GetTeamResponseDto>>(true, teams, "Success"));
        }

        [HttpPost("{teamId}/members")]
        public async Task<IActionResult> AddTeamMember(Guid teamId, TeamMemberRequestDto dto)
        {
            var result = await teamRepository.AddMemberInTeamAsync(
                teamId,
                dto.OrganizationMemberId,
                dto.Role
            );

            var response = new TeamMemberResponseDto
            {
                Id = result.Id,
                Role = result.Role.ToString(),
                UserName = result.OrganizationMember?.User?.UserName,
                Email = result.OrganizationMember?.Email
            };

            return Ok(new ApiResponse<TeamMemberResponseDto>(true, response, "Success"));
        }

        [HttpDelete("Remove-Team-Member/{teamMemberId}/{teamId}")]
        public async Task<IActionResult> RemoveTeamMember(Guid teamMemberId, Guid teamId)
        {
            var result = await teamRepository.RemoveTeamMemberAsync(teamId, teamMemberId);

            var Member = new TeamMemberResponseDto
            {
                Id = result.Id,
                Role = result.Role.ToString(),
                Name = result.OrganizationMember.User.FirstName,
                UserName = result.OrganizationMember.User.UserName,
                avatarUrl = result.OrganizationMember.User.AvatarUrl,
                Email = result.OrganizationMember.User.Email,

            };

            return Ok(new ApiResponse<TeamMemberResponseDto>(true, Member, "Success"));

        }

        [HttpPost("Archive-Team/{TeamId}")]
        public async Task<IActionResult> ArchiveTeam(Guid TeamId)
        {
            var result = await teamRepository.UpdateStatus(TeamId, TeamStatus.Archived);
            var Member = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Member, "Success"));
        }

        [HttpPost("Active-Team/{TeamId}")]
        public async Task<IActionResult> ActiveTeam(Guid TeamId)
        {
            var result = await teamRepository.UpdateStatus(TeamId, TeamStatus.Active);
            var Team = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Team, "Success"));
        }

        [HttpDelete("Soft-Delete-Team/{teamId}")]
        public async Task<IActionResult> SoftDelete(Guid teamId)
        {
            var result = await teamRepository.DeleteTeamAsync(teamId);
            var Team = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Team, "Success"));

        }

        [HttpGet("Get-TeamMember-by-Email")]
        public async Task<IActionResult> GetTeamByEmail([FromBody] string Email)
        {
            var result = await teamRepository.GetTeamMemberByEmailAsync(Email);
            var response = new TeamMemberResponseDto
            {
                Id = result.Id,
                Role = result.Role.ToString(),
                UserName = result.OrganizationMember?.User?.UserName,
                Email = result.OrganizationMember?.Email
            };

            return Ok(new ApiResponse<TeamMemberResponseDto>(true, response, "Success"));

        }

        [HttpGet("Get-TeamMember-by-Id/{Id}")]
        public async Task<IActionResult> GetTeamById(Guid Id)
        {
            var result = await teamRepository.GetTeamByIdAsync(Id);
            var Team = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Team, "Success"));

        }

        [HttpGet("Get-TeamMember-by-Name")]
        public async Task<IActionResult> GetTeamByName(string Name)
        {
            var result = await teamRepository.GetTeamByNameAsync(Name);
            var Team = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Team, "Success"));

        }

        [HttpPatch("Update-Team/{teamId}")]
        public async Task<IActionResult> UpdateTeam(Guid teamId, string Name, string Description)
        {
            var result = await teamRepository.UpdateTeamAsync(teamId, Name, Description);
            var Team = new GetTeamResponseDto
            {
                Id = result.Id,
                OrganizationId = result.OrganizationId,
                Name = result.Name,
                Description = result.Description,
                Status = result.Status.ToString(),
                CreatedByUserId = result.CreatedByUserId,
                CreatedAt = result.CreatedAt,
                TeamMembers = result.Members.Select(m => new TeamMemberResponseDto
                {
                    Id = m.Id,
                    Role = m.Role.ToString(),
                    Name = m.OrganizationMember.User.FirstName,
                    UserName = m.OrganizationMember.User.UserName,
                    avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                    Email = m.OrganizationMember.User.Email
                }).ToList()
            };

            return Ok(new ApiResponse<GetTeamResponseDto>(true, Team, "Success"));
        }
    }
}