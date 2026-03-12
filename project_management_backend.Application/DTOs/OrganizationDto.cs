using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Application.Dto
{
    public class OrganizationDto
    {
        public Guid Id { get; set;}
        public  string Name {get;set;}
        public OrganizationStatus Status {get;set;}
        public List<OrganizationDto> members {get; set;}


        
    }
}