using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Pages",
                url: "{slug}.html",
                defaults: new { controller = "Post", action = "Display" }
            );

            routes.MapRoute(
                name: "Permalink",
                url: "posts/{id}",
                defaults: new { controller = "Post", action = "Permalink" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "LogOn" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}