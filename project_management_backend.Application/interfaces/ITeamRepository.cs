using project_management_backend.Application.Dto.Team;
using project_management_backend.Domain.Entities.Project;
using project_management_backend.Domain.Entities.TeamMembers;
using project_management_backend.Domain.Entities.Teams;

namespace project_management_backend.Application.Interface
{
    public interface ITeamRepository
    {
        Task<Team> CreateTeamAsync(Team team);
        Task<Team> AddMemberInTeamAsync(TeamMember teamMember);
        Task<Team> UpdateTeamAsync(Team team);
       Task<List<GetTeamResponseDto>> GetAllTeamByOrganizationAsync(Guid orgId);
        Task<Team?> GetTeamByIdAsync(Guid Id);
        Task<TeamMember?> GetTeamMemberByIdAsync(Guid Id);
        Task<TeamMember?> GetTeamMemberByNameAsync(Guid Id);
        Task<TeamMember?> GetTeamMemberByEmailAsync(Guid Id);
        Task<Team> DeleteTeamAsync(Guid Id);
        Task<Team> ArchiveTeamAsync(Guid Id);
        Task<Project> RemoveMemberAsync(Guid projectId, Guid teamMemberId);

    }
}