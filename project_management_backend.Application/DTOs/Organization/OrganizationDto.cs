using System.ComponentModel.DataAnnotations;

namespace project_management_backend.Application.Dto.Organization
{
    public class CreateOrganizationRequestDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-z0-9-]+$")]
        public required string Slug { get; set; }
    }
    public class CreateOrganizationResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}