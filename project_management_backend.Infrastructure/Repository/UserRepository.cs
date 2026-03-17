using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Dto.user;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Organization;
using project_management_backend.Domain.Entities.Users;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public UserRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteUserAsync(Guid userId)
        {
            var user = await dbContext.User.FindAsync(userId);
            if (user == null) return null;

            dbContext.User.Remove(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<IReadOnlyList<OrganizationMember>> GetOrganizationUsersAsync(Guid organizationId)
        {
            return await dbContext.OrganizationMembers.Where(m => m.OrganizationId == organizationId).Include(m => m.User).ToListAsync();
        }

        public async Task<User?> GetUserAsync(Guid userId)
        {
            var user = await dbContext.User.FindAsync(userId);
            if (user == null) return null;
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(i => i.Email == email);
            if (user == null) return null;

            return user;
        }

        public async Task<User?> GetUserByUserNameAsync(string username)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(i => i.UserName == username);
            if (user == null) return null;

            return user;
        }

        public async Task<bool> IsUserInOrganizationAsync(Guid userId, Guid organizationId)
        {
            return await dbContext.OrganizationMembers.AnyAsync(m => m.OrganizationId == organizationId && m.UserId == userId);

        }

        public async Task<IReadOnlyList<User>> SearchOrganizationUsersAsync(Guid organizationId, string searchTerm)
        {
            var user = await dbContext.OrganizationMembers.Where(
                m => m.OrganizationId == organizationId &&
                 (m.User.FirstName.Contains(searchTerm) ||
                 m.User.LastName == null ? false : m.User.LastName.Contains(searchTerm) ||
                 m.User.Email.Contains(searchTerm))).Select(m => m.User).ToListAsync();

            return user;
        }

        public async Task<User?> SetUserActiveStatusAsync(Guid userId, bool isActive)
        {
            var userInDb = await dbContext.User.FindAsync(userId);
            if (userInDb == null) return null;
            userInDb.ChangeActiveStatus(isActive);
            await dbContext.SaveChangesAsync();
            return userInDb;
        }

        public async Task<User?> UpdateUserAsync(Guid Id, UpdateRequestDto user)
        {
            var userInDb = await dbContext.User.FindAsync(Id);
            if (userInDb == null) return null;
            userInDb.UpdateUser(user.FirstName, user.LastName, user.UserName, user.AvatarUrl);
            await dbContext.SaveChangesAsync();

            return userInDb;
        }
    }
}