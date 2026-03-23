using project_management_backend.Application.Dto.user;

namespace project_management_backend.Application.Dto
{
    public class GetOrganizationResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }

        public GetUserResponseDto? User { get; set; } 
        public Guid OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}