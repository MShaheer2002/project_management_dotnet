using System.ComponentModel.DataAnnotations;

namespace project_management_backend.Application.Dto.user
{
    public class UpdateRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be 2-50 characters")]
        public required string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name can be up to 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]{3,20}$", ErrorMessage = "Username must be 3-20 chars, letters, numbers, underscores only")]
        public required string UserName { get; set; }

        [Url(ErrorMessage = "AvatarUrl must be a valid URL")]
        public string? AvatarUrl { get; set; }
    }

    public class UpdateResponseDto
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}