using System.ComponentModel.DataAnnotations;
using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Application.Dto
{
    public class InviteResponseDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required OrganizationRole Role { get; set; }
    }
}