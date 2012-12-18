using System.Web.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles.Administrator)]
    public class AdminController : Controller
    {
    }
}
