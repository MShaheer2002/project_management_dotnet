using project_management_backend.Domain.Entities.Users;

namespace project_management_backend.Domain.Entities.Department
{
    public class DepartmentMember
    {
        public Guid Id { get; private set; }

        public Guid DepartmentId { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid WorkspaceMemberId { get; private set; }

        public DepartmentRole Role { get; private set; }

        public Guid AddedBy { get; private set; }
        public DateTime JoinedAt { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsRemoved { get; private set; }

        public Guid? RemovedBy { get; private set; }
        public Guid? UpdatedBy { get; private set; }
        public DateTime? RemovedAt { get; private set; }

        // EF Core
        private DepartmentMember() { }

        public DepartmentMember(
            Guid departmentId,
            Guid userId,
            Guid workspaceMemberId,
            DepartmentRole role,
            Guid addedBy)
        {
            if (departmentId == Guid.Empty)
                throw new ArgumentException("DepartmentId is required");

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required");

            Id = Guid.NewGuid();

            DepartmentId = departmentId;
            UserId = userId;
            WorkspaceMemberId = workspaceMemberId;

            Role = role;

            AddedBy = addedBy;
            JoinedAt = DateTime.UtcNow;

            IsActive = true;
            IsRemoved = false;
        }

        public void ChangeRole(DepartmentRole role, Guid updatedBy)
        {
            Role = role;
            UpdatedBy = updatedBy;

        }

        public void Remove(Guid removedBy)
        {
            if (IsRemoved) return;

            IsRemoved = true;
            IsActive = false;

            RemovedBy = removedBy;
            RemovedAt = DateTime.UtcNow;
        }

        public void Reactivate(Guid updatedBy)
        {
            if (!IsRemoved) return;

            IsRemoved = false;
            IsActive = true;
            UpdatedBy = updatedBy;

            RemovedBy = null;
            RemovedAt = null;

        }
    }

    public enum DepartmentRole
    {
        Member,
        Lead,
        Manager
    }
}