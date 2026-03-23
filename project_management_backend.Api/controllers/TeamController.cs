using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto.Team;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
    }
}