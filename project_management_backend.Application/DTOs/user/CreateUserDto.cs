using System.ComponentModel.DataAnnotations;

namespace project_management_backend.Application.Dto.user
{
    public class CreateUserRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be 2-50 characters")]
        public required string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name can be up to 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]{3,20}$", ErrorMessage = "Username must be 3-20 chars, letters, numbers, underscores only")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public required string Password { get; set; }

        [Url(ErrorMessage = "AvatarUrl must be a valid URL")]
        public string? AvatarUrl { get; set; }
    }

    public class CreateUserResponseDto
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? AvatarUrl { get; set; }
    }
}