

using Microsoft.EntityFrameworkCore;
using project_management_backend.Application.Dto.Team;
using project_management_backend.Application.Interface;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;
using project_management_backend.Infrastructure.Migrations;
using project_management_backend.Infrastructure.Persistence;

namespace project_management_backend.Infrastructure.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ProjectManagementDbContext dbContext;

        public TeamRepository(ProjectManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<Team> AddMemberInTeamAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public Task<Team> ArchiveTeamAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            await dbContext.Teams.AddAsync(team);
            await dbContext.SaveChangesAsync();
            return team;
        }

        public Task<Team> DeleteTeamAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetTeamResponseDto>> GetAllTeamByOrganizationAsync(Guid orgId)
        {
            var team = await dbContext.Teams
                .Select(t => new GetTeamResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    OrganizationId = t.OrganizationId,
                    Description = t.Description,
                    CreatedByUserId = t.CreatedByUserId,
                    CreatedAt = t.CreatedAt,
                    Status = t.Status.ToString(),

                    TeamMembers = t.Members.Select(m => new TeamMemberResponseDto
                    {
                        OrganizationMemberId = m.OrganizationMemberId,
                        Role = m.Role.ToString(),

                        UserName = m.OrganizationMember.User.UserName,
                        Email = m.OrganizationMember.User.Email
                    }).ToList()
                }).ToListAsync();
            return team;
        }

        public Task<Team?> GetTeamByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamMember?> GetTeamMemberByEmailAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamMember?> GetTeamMemberByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamMember?> GetTeamMemberByNameAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Project> RemoveMemberAsync(Guid projectId, Guid teamMemberId)
        {
            throw new NotImplementedException();
        }

        public Task<Team> UpdateTeamAsync(Team team)
        {
            throw new NotImplementedException();
        }
    }
}