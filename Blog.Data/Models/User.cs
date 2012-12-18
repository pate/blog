using System;
using System.Collections.Generic;
using DreamSongs.MongoRepository;

namespace Blog.Data.Models
{
    [Serializable]
    public class UserRoleAssignment
    {
        public string Name { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AssignedBy { get; set; }
    }

    [Serializable]
    public class User : Entity
    {
        //public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockedOutDate { get; set; }
        public List<UserRoleAssignment> RoleAssignment { get; set; }
    }
}