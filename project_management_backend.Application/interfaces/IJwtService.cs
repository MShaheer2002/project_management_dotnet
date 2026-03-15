namespace project_management_backend.Application.Interface
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId,string email);
    }
}