using project_management_backend.Application.Dto.Team;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.Application.Interface
{
    public interface ITeamRepository
    {
        Task<Team> CreateTeamAsync(Team team);
        Task<TeamMember> AddMemberInTeamAsync(Guid teamId, Guid organizationMemberId, TeamRole role);
        Task<Team> UpdateTeamAsync(Guid Id, string Name, string Description);
        Task<List<GetTeamResponseDto>> GetAllTeamByOrganizationAsync(Guid orgId);
        Task<Team?> GetTeamByIdAsync(Guid Id);
        Task<Team?> GetTeamByNameAsync(string Name);
        Task<TeamMember?> GetTeamMemberByIdAsync(Guid Id);
        Task<List<TeamMember>> GetTeamMembersByNameAsync(Guid teamId, string name);
        Task<TeamMember?> GetTeamMemberByEmailAsync(String email);
        Task<Team> DeleteTeamAsync(Guid Id);
        Task<Team> UpdateStatus(Guid Id, TeamStatus status);
        Task<TeamMember> RemoveTeamMemberAsync(Guid teamId, Guid teamMemberId);

    }
}