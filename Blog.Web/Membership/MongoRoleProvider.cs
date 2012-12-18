using System;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Mvc;
using Blog.Data.Models;
using Blog.Web.Helpers;
using DreamSongs.MongoRepository;

namespace Blog.Web.Membership
{
    public class MongoRoleProvider : System.Web.Security.RoleProvider
    {
        protected IRepository<User> Users;
        protected IRepository<Role> Roles;

        protected string DefaultAdminUsername = string.Empty;
        protected string DefaultAdminRole = string.Empty;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            Users = DependencyResolver.Current.GetService<IRepository<User>>();
            Roles = DependencyResolver.Current.GetService<IRepository<Role>>();

            base.Initialize(name, config);

            if (config.AllKeys.Contains("DefaultAdminUser"))
            {
                DefaultAdminUsername = config["DefaultAdminUser"];
            }
            if (config.AllKeys.Contains("DefaultAdminRole"))
            {
                DefaultAdminRole = config["DefaultAdminRole"];
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] role_names)
        {
            foreach (string username in usernames)
            {
                if (string.IsNullOrEmpty(username)) throw new ArgumentException();

                var user = Users.All().First(x => x.Username == username);

                if (user == null) throw new ProviderException();

                foreach (string roleName in role_names)
                {
                    if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

                    var role = Roles.All().FirstOrDefault(x => x.Name ==  roleName);

                    if (role == null) throw new ArgumentException();

                    if (!user.RoleAssignment.Any(x => x.Name == roleName))
                    {
                        user.RoleAssignment.Add(new UserRoleAssignment() { Name = roleName, AssignedDate = DateTime.Now });
                    }

                    Users.Update(user);
                }
            }
        }

        public override string ApplicationName
        {
            get
            {
                return "/";
            }
            set
            {
            }
        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

            if (Roles.All().Count(x => x.Name == roleName) > 0)
                throw new ProviderException();

            var role = new Role
                           {
                               Name = roleName,
                               Description = string.Empty
                           };

            Roles.Add(role);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

            var role = Roles.All().FirstOrDefault(x => x.Name == roleName);

            if (role == null) throw new ProviderException();

            if (throwOnPopulatedRole)
            {
                if ((from u in Users.All() where u.RoleAssignment.Any(x => x.Name == roleName) select u).Any())
                {
                    throw new ProviderException();
                }
            }

            foreach (var user in (from u in Users.All() where u.RoleAssignment.Any(x => x.Name == roleName) select u))
            {
                user.RoleAssignment.RemoveAll(x => x.Name == roleName);

                Users.Update(user);
            }

            Roles.Delete(role);
            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return GetUsersInRole(roleName);
        }

        public override string[] GetAllRoles()
        {
            return Roles.All().Select(x => x.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException();

            if (!string.IsNullOrEmpty(DefaultAdminUsername))
            {
                if (DefaultAdminUsername == username)
                {
                    return new string[] { DefaultAdminRole };
                }
            }

            var user = Users.All().FirstOrDefault(x => x.Username == username);

            if (user == null) throw new ProviderException();

            return (from r in user.RoleAssignment.NullToEmpty() select r.Name).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

            var role = Roles.All().FirstOrDefault(x => x.Name == roleName);

            if (role == null) throw new ProviderException();

            return (from u in Users.All()
                    where u.RoleAssignment.Any(x => x.Name == roleName)
                    select u.Username).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException();

            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

            if (!string.IsNullOrEmpty(DefaultAdminUsername) && !string.IsNullOrEmpty(DefaultAdminRole))
            {
                if (username == DefaultAdminUsername && roleName == DefaultAdminRole)
                {
                    return true;
                }
            }

            var user = Users.All().FirstOrDefault(x => x.Username == username);

            if (user == null) throw new ProviderException();

            var role = Roles.All().FirstOrDefault(x => x.Name == roleName);

            if (role == null) throw new ProviderException();

            return user.RoleAssignment.Any(x => x.Name == roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] role_names)
        {
            foreach (string username in usernames)
            {
                if (string.IsNullOrEmpty(username)) throw new ArgumentException();

                var user = Users.All().FirstOrDefault(x => x.Username == username);

                if (user == null) throw new ProviderException();

                foreach (string roleName in role_names)
                {
                    if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();

                    var role = Roles.All().FirstOrDefault(x => x.Name == roleName);

                    if (role == null) throw new ArgumentException();

                    if (user.RoleAssignment.Any(x => x.Name == roleName))
                    {
                        user.RoleAssignment.RemoveAll(x => x.Name == roleName);
                    }

                    Users.Update(user);
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            return Roles.All().Any(x => x.Name == roleName);
        }
    }
}