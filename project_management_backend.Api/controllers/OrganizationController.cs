using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Dto;
using project_management_backend.Application.Repository;
using project_management_backend.Domain.Organization;
using project_management_backend.Infrastructure.Data;

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
    };
}