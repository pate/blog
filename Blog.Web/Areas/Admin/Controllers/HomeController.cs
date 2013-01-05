using System.Web.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
