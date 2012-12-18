using System.Web.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    public class RoleController : AdminController
    {
        public ActionResult Index()
        {
            var roles = System.Web.Security.Roles.GetAllRoles();

            return View(roles);
        }
/*
        public ActionResult Create(string id)
        {
            System.Web.Security.Roles.CreateRole(id);

            this.FlashInfo("Created Role {0}".Fmt(id));

            return RedirectToAction("Index");
        }*/
    }
}
