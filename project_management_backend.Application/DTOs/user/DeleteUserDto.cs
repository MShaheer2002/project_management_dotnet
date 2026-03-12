namespace project_management_backend.Application.Dto.user
{
    public class DeleteUserDto
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}