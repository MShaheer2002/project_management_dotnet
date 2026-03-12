using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Dto;
using project_management_backend.Application.Interface;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository organizationRepository;
        private readonly ProjectManagementDbContext dbContext;

        public OrganizationController(IOrganizationRepository organizationRepository, ProjectManagementDbContext dbContext)
        {
            this.organizationRepository = organizationRepository;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizationDto = await dbContext.Organizations.Select(o => new OrganizationDto()
            {

                Id = o.Id,
                Name = o.Name ?? "unkown",
                Status = o.Status,
                members = (List<OrganizationDto>)o.Members,

            }).ToListAsync();

            return Ok(organizationDto);
        }

        [HttpGet("slug-availability")]
        public async Task<IActionResult> SlugAvailability(string slug)
        {
            var avaible = await organizationRepository.IsSlugAvaibleAsync(slug);

            var response = new ApiResponse<Object>(true, new { slug, avaible }, "workspace url available");
            return Ok(response);

        }
    };
}