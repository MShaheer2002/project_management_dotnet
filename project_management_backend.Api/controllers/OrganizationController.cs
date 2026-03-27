using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto;
using project_management_backend.Application.Dto.Organization;
using project_management_backend.Application.Dto.user;
using Microsoft.Extensions.Logging;
using project_management_backend.Application.Interface;
using project_management_backend.Infrastructure.Repository;
using project_management_backend.Domain.Entities.Organizations;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository organizationRepository;
        private readonly IOrganizationMemberRepository organizationMemberRepository;
        private readonly ILogger<OrganizationRepository> logger;

        public OrganizationController(IOrganizationRepository organizationRepository, IOrganizationMemberRepository organizationMemberRepository, ILogger<OrganizationRepository> logger)
        {
            this.organizationRepository = organizationRepository;
            this.organizationMemberRepository = organizationMemberRepository;
            this.logger = logger;
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
                Name = o.Name ?? "",
                Slug = o.Slug ?? "",
                User = o.Owner != null ? new GetUserResponseDto
                {
                    Id = o.Owner.Id,
                    FirstName = o.Owner.FirstName,
                    LastName = o.Owner.LastName,
                    UserName = o.Owner.UserName,
                    AvatarUrl = o.Owner.AvatarUrl,
                } : null,
                OwnerUserId = o.OwnerUserId,
                CreatedAt = o.CreatedAt,
            }).ToList();

            return Ok(new ApiResponse<List<GetOrganizationResponseDto>>(true, organization, "Success"));
        }

        [HttpGet("{orgId}")]
        public async Task<IActionResult> GetById(Guid orgId)
        {
            var organization = await organizationRepository.GetByIdAsync(orgId);

            if (organization == null)
            {
                return NotFound(new ApiResponse<Object>(false, "", "Organization Not Found!"));
            }

            var data = new GetOrganizationResponseDto
            {
                Id = organization.Id,
                Name = organization.Name ?? "",
                Slug = organization.Slug ?? "",
                CreatedAt = organization.CreatedAt,

            };

            return Ok(new ApiResponse<GetOrganizationResponseDto>(true, data, "Organization Found"));
        }

        [HttpPost("{orgId}/invite")]
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
        [HttpGet("accept-invite")]
        public async Task<IActionResult> AcceptInvite([FromQuery] string token)
        {
            var userIdInvite = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdInvite == null) return Unauthorized();
            var userId = Guid.Parse(userIdInvite);


            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail)) return BadRequest(new ApiResponse<Object>(false, "", "User email not found in token"));

            await organizationMemberRepository.AcceptInviteAsync(token, userId, userEmail);

            return Ok(new ApiResponse<Object>(true, "", "Invitation Accepted"));
        }

        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeRole(Guid memberId, OrganizationRole role)
        {
            var userIdInvite = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdInvite == null) return Unauthorized();
            var userId = Guid.Parse(userIdInvite);

            var member = await organizationMemberRepository.ChangeRoleAsync(memberId, newRole: role, currentUserId: userId);
            return Ok(new ApiResponse<Object>(true, member, "Invitation Accepted"));
        }

        [HttpPost("deactive/{orgId}")]
        public async Task<IActionResult> Deactive(Guid orgId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

            var org = await organizationRepository.DeactiveAsync(userId, orgId);
            if (org == null) return NotFound(new ApiResponse<Object>(false, "", "Organization not found"));

            var organization = new DeleteOrganizationResponseDto
            {
                Id = org.Id,
                Name = org.Name ?? "",
                Slug = org.Slug ?? "",
                Status = org.Status,
                CreatedAt = org.CreatedAt,
            };

            return Ok(new ApiResponse<DeleteOrganizationResponseDto>(true, organization, "Org Deactivated"));

        }

        [HttpPost("remove-memeber/{orgId}/{deleteUserId}")]
        public async Task<IActionResult> RemoveMember(Guid orgId, Guid deleteUserId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);
            await organizationRepository.RemoveMemberAsync(orgId, userId, deleteUserId);

            return Ok(new ApiResponse<Object>(true, "", "User Removed"));

        }

        [HttpGet("get-by-slug")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var org = await organizationRepository.GetBySlugAsync(slug);

            if (org == null) return NotFound(new ApiResponse<Object>(false, "", "No Organization Found"));

            var organization = new GetOrganizationResponseDto
            {
                Id = org.Id,
                Name = org.Name ?? "",
                Slug = org.Slug ?? "",
                CreatedAt = org.CreatedAt,
            };

            return Ok(new ApiResponse<Object>(true, organization, "Organization Found"));

        }

        [HttpGet("get-by-userId")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var orgs = await organizationRepository.GetUserOrganizationsAsync(userId);

            if (orgs == null || !orgs.Any())
                return NotFound(new ApiResponse<object>(false, null, "No organizations found"));

            var response = orgs.Select(org => new GetOrganizationResponseDto
            {
                Id = org.Id,
                Name = org.Name ?? "",
                Slug = org.Slug ?? "",
                CreatedAt = org.CreatedAt,
            }).ToList();
            return Ok(new ApiResponse<List<GetOrganizationResponseDto>>(true, response, "Organizations retrieved successfully"));
        }


    };
}