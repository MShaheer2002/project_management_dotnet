using project_management_backend.Domain.Entities.Organizations;

namespace project_management_backend.Application.Dto.Organization
{
    public class DeleteOrganizationResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required OrganizationStatus Status {get;set;}
        public DateTime CreatedAt { get; set; }
    }
}