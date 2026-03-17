using project_management_backend.Application.Dto.user;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Domain.Entities.Users;

namespace project_management_backend.Application.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> DeleteUserAsync(Guid userId);
        Task<User?> UpdateUserAsync(Guid Id,UpdateRequestDto user);
        Task<User?> GetUserAsync(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserNameAsync(string username);
        Task<IReadOnlyList<OrganizationMember>> GetOrganizationUsersAsync(Guid organizationId);
        Task<bool> IsUserInOrganizationAsync(Guid userId, Guid organizationId);
        Task<IReadOnlyList<User>> SearchOrganizationUsersAsync(Guid organizationId, string searchTerm);
        Task<User?> SetUserActiveStatusAsync(Guid userId, bool isActive);
    }
}