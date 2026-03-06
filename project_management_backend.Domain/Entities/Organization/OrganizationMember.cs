using System.ComponentModel.DataAnnotations;

namespace project_management_backend.Domain.Entities.Organization
{


 public class OrganizationMember
{
    [Key]
    public Guid OrganizationMemberId { get; private set; }
    public Guid UserId { get; private set; }

    public OrganizationRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private OrganizationMember() { }

    public OrganizationMember( Guid userId, OrganizationRole role)
    {
        OrganizationMemberId = Guid.NewGuid();
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    public void ChangeRole(OrganizationRole role)
    {
        Role = role;
    }
}
    public enum OrganizationRole
    {
        Owner = 0,
        Admin = 1,
        Member = 2
    }
}