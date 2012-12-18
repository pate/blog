using System;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Blog.Data.Models;
using DreamSongs.MongoRepository;

namespace Blog.Web.Membership
{
    public class MongoMembershipProvider : MembershipProvider
    {
        protected IRepository<User> Users;

        protected string DefaultAdminUsername = string.Empty;

        protected MembershipUser GetMembershipUser(User userRecord)
        {
            return new MembershipUser("MongoMembershipProvider",
                userRecord.Username,
                userRecord.Id,
                userRecord.Email,
                userRecord.PasswordQuestion,
                userRecord.Comment,
                userRecord.IsApproved,
                userRecord.IsLockedOut,
                userRecord.CreationDate,
                userRecord.LastLoginDate,
                userRecord.LastLockedOutDate,
                userRecord.LastPasswordChangedDate,
                userRecord.LastLockedOutDate);
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            Users = DependencyResolver.Current.GetService<IRepository<User>>();

            base.Initialize(name, config);

            if (config.AllKeys.Contains("DefaultAdminUser"))
            {
                DefaultAdminUsername = config["DefaultAdminUser"];
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            string oldPasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "sha1");
            string newPasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

            var user = (from u in Users.All() where u.Username == username && u.PasswordHash == oldPasswordHash select u).FirstOrDefault();

            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                Users.Update(user);
                return true;
            }

            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            var user = (from u in Users.All() where u.Username == username && u.PasswordHash == passwordHash select u).FirstOrDefault();

            if (user != null)
            {
                user.PasswordQuestion = newPasswordQuestion;
                user.PasswordAnswer = newPasswordAnswer;
                Users.Update(user);
                return true;
            }

            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (Users.All().Any(x => x.Username == username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (Users.All().Any(x => x.Email.ToUpper() == email.ToUpper()))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            var user = new User
                           {
                               Username = username,
                               Email = email,
                               PasswordHash = passwordHash,
                               PasswordQuestion = passwordQuestion,
                               PasswordAnswer = passwordAnswer,
                               IsApproved = isApproved,
                               Comment = string.Empty,
                               IsLockedOut = false,
                               CreationDate = DateTime.Now,
                               LastLoginDate = DateTime.Now,
                               LastActivityDate = DateTime.Now,
                               LastLockedOutDate = DateTime.Now,
                               LastPasswordChangedDate = DateTime.Now
                           };

            Users.Add(user); // not sure if this sets id

            status = MembershipCreateStatus.Success;

            return GetMembershipUser(user);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var user = (from u in Users.All() where u.Username == username select u).FirstOrDefault();

            if (user != null)
            {
                Users.Delete(user);
                return true;
            }
            return false;
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = Users.All().Count(u => u.Email == emailToMatch);

            var collection = new MembershipUserCollection();

            foreach (var user in (from u in Users.All() where u.Email == emailToMatch select u).Skip(pageIndex * pageSize).Take(pageSize))
            {
                collection.Add(GetMembershipUser(user));
            }

            return collection;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = Users.All().Count(u => u.Username == usernameToMatch);

            var collection = new MembershipUserCollection();

            foreach (var user in (from u in Users.All() where u.Username == usernameToMatch select u).Skip(pageIndex * pageSize).Take(pageSize))
            {
                collection.Add(GetMembershipUser(user));
            }

            return collection;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = Users.All().Count();

            var collection = new MembershipUserCollection();

            foreach (var user in (from u in Users.All() select u).Skip(pageIndex * pageSize).Take(pageSize))
            {
                collection.Add(GetMembershipUser(user));
            }

            return collection;
        }

        public override int GetNumberOfUsersOnline()
        {
            return Users.All().Count(u => u.LastActivityDate >= DateTime.Now.AddMinutes(-15));
        }

        public override string GetPassword(string username, string answer)
        {
            throw new ProviderException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = Users.All().FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.Now;
                    Users.Update(user);
                }

                return GetMembershipUser(user);
            }

            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var key = (string)providerUserKey;

            var user = Users.GetById(key);

            if (user != null)
            {
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.Now;
                    Users.Update(user);
                }

                return GetMembershipUser(user);
            }

            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            return Users.All()
                .Where(u => u.Email == email)
                .Select(x => x.Username)
                .FirstOrDefault();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 1; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 10; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return string.Empty; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return true; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset) throw new NotSupportedException();

            var user = Users.All().FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                if (RequiresQuestionAndAnswer)
                {
                    if (user.PasswordAnswer != answer) throw new MembershipPasswordException();
                }

                string newPassword = System.Web.Security.Membership.GeneratePassword(12, 4);

                string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

                user.PasswordHash = passwordHash;

                Users.Update(user);

                return newPassword;
            }

            throw new MembershipPasswordException();
        }

        public override bool UnlockUser(string userName)
        {
            var user = Users.All().FirstOrDefault(u => u.Username == userName);

            if (user != null)
            {
                user.IsLockedOut = false;
                Users.Update(user);
                return true;
            }

            return false;
        }

        public override void UpdateUser(MembershipUser membershipUser)
        {
            var key = (string)membershipUser.ProviderUserKey;

            var user = Users.GetById(key);

            if (user != null)
            {
                user.Username = membershipUser.UserName;
                user.Email = membershipUser.Email;
                user.PasswordQuestion = membershipUser.PasswordQuestion;
                user.Comment = membershipUser.Comment;
                user.IsApproved = membershipUser.IsApproved;
                user.IsLockedOut = membershipUser.IsLockedOut;
                user.CreationDate = membershipUser.CreationDate;
                user.LastLoginDate = membershipUser.LastLoginDate;
                user.LastActivityDate = membershipUser.LastActivityDate;
                user.LastPasswordChangedDate = membershipUser.LastPasswordChangedDate;
                user.LastLockedOutDate = membershipUser.LastLockoutDate;

                Users.Update(user);
            }
        }

        public override bool ValidateUser(string username, string password)
            // vulnerable to timing attacks?
        {
            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            var user = Users.All().FirstOrDefault(u => u.Username == username);

            if (user == null)
                return false;

            if (user.PasswordHash == passwordHash)
                return true;

            return false;
        }
    }
}