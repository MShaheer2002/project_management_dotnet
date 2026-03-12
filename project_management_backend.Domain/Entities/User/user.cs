using System;
using System.Collections.Generic;
using project_management_backend.Domain.Entities.Organization;

namespace project_management_backend.Domain.Entities.User
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }

        public string? AvatarUrl { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public ICollection<OrganizationMember> OrganizationMembers { get; private set; } = new List<OrganizationMember>();

        private User() { } // For EF

        public User(string firstName, string? lastName, string email, string username, string passwordHash, string? avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("User must have a name.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("User must have an email.");

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName ?? "";
            Email = email;
            UserName = username;
            PasswordHash = passwordHash;
            AvatarUrl = avatarUrl ?? "";
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateUser(string firstName, string? lastName,string userName, string? avatarUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName ?? "";
            AvatarUrl = avatarUrl ?? "";
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeActiveStatus(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}