namespace ProjectManagementBackend.Application.Dto.auth
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponseDto
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public string? AvatarUrl { get; set; }

        public required string Email { get; set; }
        public required string JoinedAt { get; set; }

    }

}

