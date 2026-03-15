using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto;
using project_management_backend.Application.Dto.Organization;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository organizationRepository;

        public OrganizationController(IOrganizationRepository organizationRepository)
        {
            this.organizationRepository = organizationRepository;
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
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var org = new Organization(
                organizationRequestDto.Name,
                organizationRequestDto.Slug,
                userId
            );

            var isSlugAvaible = await organizationRepository.IsSlugAvaibleAsync(organizationRequestDto.Slug);

            if (!isSlugAvaible) return BadRequest(new ApiResponse<Object>(false, isSlugAvaible, "Slug already taken"));

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
    };
}