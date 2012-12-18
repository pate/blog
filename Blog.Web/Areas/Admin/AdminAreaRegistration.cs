using System.Web.Mvc;

namespace Blog.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return Constants.Areas.Admin;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "Page", id = UrlParameter.Optional },
                namespaces: new[] { "Blog.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
