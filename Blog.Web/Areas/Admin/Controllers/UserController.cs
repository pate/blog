using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Blog.Web.Helpers;

namespace Blog.Web.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        public ActionResult Index()
        {
            var users = System.Web.Security.Membership.GetAllUsers();

            return View(users);
        }

        public ActionResult Details(string id)
        // where id is username
        {
            var user = System.Web.Security.Membership.GetUser(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpGet]
        public ActionResult Edit(string id)
            // where id is username
        {
            var user = System.Web.Security.Membership.GetUser(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(string id, MembershipUser input)
        // where id is username
        {
            var user = System.Web.Security.Membership.GetUser(id);
            if (user == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                Mapper.Map(input, user);

                System.Web.Security.Membership.UpdateUser(user);

                return RedirectToAction("Details", new {id});
            }

            return View(user);
        }

        

        [HttpGet]
        public ActionResult Delete(string id)
            // where id is username
        {
            var user = System.Web.Security.Membership.GetUser(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
            // where id is username
        {
            var user = System.Web.Security.Membership.GetUser(id);
            if (user == null)
                return HttpNotFound();

            System.Web.Security.Membership.DeleteUser(user.UserName);

            this.FlashInfo("Deleted user " + user.UserName);

            return RedirectToAction("Index");
        }

        //public ActionResult Temp(string id, string role)
        //{
        //    System.Web.Security.Roles.AddUserToRole(id, role);

        //    this.FlashInfo("Added user {0} to role {1}".Fmt(id, role));
        //    return RedirectToAction("Index");
        //}
    }
}
