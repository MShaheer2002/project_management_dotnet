using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto;
using project_management_backend.Application.Dto.Organization;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository organizationRepository;
        private readonly IOrganizationMemberRepository organizationMemberRepository;

        public OrganizationController(IOrganizationRepository organizationRepository, IOrganizationMemberRepository organizationMemberRepository)
        {
            this.organizationRepository = organizationRepository;
            this.organizationMemberRepository = organizationMemberRepository;
        }


        [HttpGet("slug-availability")]
        public async Task<IActionResult> SlugAvailability(string slug)
        {
            var avaible = await organizationRepository.IsSlugAvaibleAsync(slug);

            var response = new ApiResponse<Object>(true, new { slug, avaible }, "workspace url available");
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganizationRequestDto organizationRequestDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

            var slug = organizationRequestDto.Slug.Trim().ToLower();

            var isSlugAvaible = await organizationRepository.IsSlugAvaibleAsync(slug);

            if (!isSlugAvaible) return Conflict(new ApiResponse<Object>(false, isSlugAvaible, "Slug already taken"));

            var org = new Organization(
                organizationRequestDto.Name,
                organizationRequestDto.Slug,
                userId
            );

            var createOrg = await organizationRepository.CreateAsync(org);

            var organization = new CreateOrganizationResponseDto
            {
                Name = createOrg.Name!,
                Slug = createOrg.Slug!,
                OwnerUserId = createOrg.OwnerUserId,
                Id = createOrg.Id,
                CreatedAt = createOrg.CreatedAt

            };

            return Ok(new ApiResponse<CreateOrganizationResponseDto>(true, organization, "Success"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orgs = await organizationRepository.GetAllAsync();

            var organization = orgs.Select(o => new GetOrganizationResponseDto
            {
                Id = o.Id,
                Name = o.Name,
                Slug = o.Slug,
                User = new GetUserResponseDto
                {
                    Id = o.Owner.Id,
                    FirstName = o.Owner.FirstName,
                    LastName = o.Owner.LastName,
                    UserName = o.Owner.UserName,
                    AvatarUrl = o.Owner.AvatarUrl,
                },
                OwnerUserId = o.OwnerUserId,
                CreatedAt = o.CreatedAt,
            }).ToList();

            return Ok(new ApiResponse<List<GetOrganizationResponseDto>>(true, organization, "Success"));
        }


        [HttpPost("{orgId}/invite")]
        [Authorize]
        public async Task<IActionResult> Invite(Guid orgId, [FromBody] InviteResponseDto dto)
        {
            var result = await organizationMemberRepository.InviteMemberAsync(orgId, dto.Email, dto.Role);

            var invited = new InviteResponseDto
            {
                Email = result.Email,
                Role = result.Role

            };
            return Ok(new ApiResponse<InviteResponseDto>(true, invited, "Success"));

        }
        [HttpPost("accept-invite")]
        [Authorize]
        public async Task<IActionResult> AcceptInvite([FromQuery] string token)
        {
            var userIdInvite = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdInvite == null) return Unauthorized();
            var userId = Guid.Parse(userIdInvite);

            var userEmail = User.FindFirst("email")?.Value;

            await organizationMemberRepository.AcceptInviteAsync(token, userId, userEmail);

            return Ok(new { message = "Joined organization successfully" });
        }


    };
}