

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project_management_backend.Application.Dto.Team;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ProjectManagementDbContext dbContext;
        private readonly ILogger<TeamRepository> logger;

        public TeamRepository(ProjectManagementDbContext dbContext, ILogger<TeamRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<TeamMember> AddMemberInTeamAsync(Guid teamId, Guid organizationMemberId, TeamRole role)
        {
            var team = await dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
                throw new KeyNotFoundException("Team Not Found");

            // Validate OrganizationMember
            var orgMember = await dbContext.OrganizationMembers
                .Include(om => om.User)
                .FirstOrDefaultAsync(om => om.Id == organizationMemberId);

            if (orgMember == null)
                throw new KeyNotFoundException("Organization Member not found");

            if (orgMember.OrganizationId != team.OrganizationId)
                throw new InvalidOperationException("Member does not belong to the same organization as the team.");

            if (team.Members.Any(m => m.OrganizationMemberId == organizationMemberId))
                throw new InvalidOperationException("Member already exists in team");

            var member = team.AddMember(organizationMemberId, role);
            await dbContext.AddAsync(member);
            await dbContext.SaveChangesAsync();

            return member;
        }

        public async Task<Team> UpdateStatus(Guid Id, TeamStatus status)
        {
            var team = await dbContext.Teams.FirstOrDefaultAsync(tm => tm.Id == Id);
            if (team == null) throw new KeyNotFoundException("Team not Found!");

            team.UpdateStatus(status);
            await dbContext.SaveChangesAsync();
            return team;
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            await dbContext.Teams.AddAsync(team);
            await dbContext.SaveChangesAsync();
            return team;
        }

        public async Task<Team> DeleteTeamAsync(Guid Id)
        {
            var team = await dbContext.Teams
               .Include(t => t.Members)
               .FirstOrDefaultAsync(t => t.Id == Id);

            if (team == null)
                throw new KeyNotFoundException("Team Not Found");

            team.SoftDelete();
            await dbContext.SaveChangesAsync();

            return team;
        }

        public async Task<List<GetTeamResponseDto>> GetAllTeamByOrganizationAsync(Guid orgId)
        {
            var team = await dbContext.Teams
                .Select(t => new GetTeamResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    OrganizationId = t.OrganizationId,
                    CreatedByUserId = t.CreatedByUserId,
                    CreatedAt = t.CreatedAt,
                    Status = t.Status.ToString(),

                    TeamMembers = t.Members.Select(m => new TeamMemberResponseDto
                    {
                        Id = m.Id,
                        Role = m.Role.ToString(),
                        Name = m.OrganizationMember.User.FirstName,
                        UserName = m.OrganizationMember.User.UserName,
                        avatarUrl = m.OrganizationMember.User.AvatarUrl ?? "",
                        Email = m.OrganizationMember.User.Email
                    }).ToList()
                }).ToListAsync();
            return team;
        }

        public async Task<Team?> GetTeamByIdAsync(Guid Id)
        {
            var team = await dbContext.Teams.FirstOrDefaultAsync(tm => tm.Id == Id);

            return team;
        }
        public async Task<Team?> GetTeamByNameAsync(string Name)
        {
            var team = await dbContext.Teams.FirstOrDefaultAsync(tm => tm.Name.ToLower() == Name.ToLower());

            return team;
        }

        public async Task<TeamMember?> GetTeamMemberByEmailAsync(String email)
        {
            var teamMember = await dbContext.TeamMembers
                .Include(tm => tm.OrganizationMember)
                    .ThenInclude(om => om.User)
                .FirstOrDefaultAsync(tm => tm.OrganizationMember.User.Email.ToLower() == email.ToLower());
            return teamMember;
        }

        public async Task<TeamMember?> GetTeamMemberByIdAsync(Guid Id)
        {
            var teamMember = await dbContext.TeamMembers
             .Include(tm => tm.OrganizationMember)
                 .ThenInclude(om => om.User)
             .FirstOrDefaultAsync(tm => tm.Id == Id);
            return teamMember;
        }

        public async Task<List<TeamMember>> GetTeamMembersByNameAsync(Guid teamId, string name)
        {
            return await dbContext.TeamMembers
                .Include(tm => tm.OrganizationMember)
                    .ThenInclude(om => om.User)
                .Where(tm =>
                    tm.TeamId == teamId &&
                    (
                        tm.OrganizationMember.User.FirstName == name ||
                        tm.OrganizationMember.User.LastName == name ||
                        tm.OrganizationMember.User.UserName == name
                    ))
                .ToListAsync();
        }

        public async Task<TeamMember> RemoveTeamMemberAsync(Guid teamId, Guid teamMemberId)
        {
            var teamMember = await dbContext.TeamMembers
                .Include(o => o.OrganizationMember)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(tm =>
                    tm.TeamId == teamId &&
                    tm.Id == teamMemberId);

            if (teamMember == null)
                throw new KeyNotFoundException("Team member not found");

            dbContext.TeamMembers.Remove(teamMember);

            await dbContext.SaveChangesAsync();
            return teamMember;
        }

        public async Task<Team> UpdateTeamAsync(Guid Id, string Name, string Description)
        {
            var Team = await dbContext.Teams.FirstOrDefaultAsync(t => t.Id == Id);
            if (Team == null) throw new KeyNotFoundException("Team Not Found!");
            Team.Update(Name, Description);
            await dbContext.SaveChangesAsync();
            return Team;

        }
    }
}