using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var homePageSlug = ConfigurationManager.AppSettings["HomePageSlug"];

            routes.MapRoute(
                name: "Home Page",
                url: "",
                defaults: new { controller = "Page", action = "Display", slug = homePageSlug },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Posts",
                url: "posts/{slug}.html",
                defaults: new { controller = "Post", action = "Display", area = "" },
                namespaces: new [] { "Blog.Web.Controllers" }
            );
            
            routes.MapRoute(
                name: "Posts Permalink",
                url: "posts/{id}",
                defaults: new { controller = "Post", action = "Permalink", area = "" },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Pages",
                url: "{slug}.html",
                defaults: new { controller = "Page", action = "Display", area = "" },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute( // this should be a redirect
                name: "Pages Redirect",
                url: "pages/{slug}.html",
                defaults: new { controller = "Page", action = "Display", area = "" },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Pages Permalink",
                url: "pages/{id}",
                defaults: new { controller = "Page", action = "Permalink", area = "" },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "LogOn" },
                namespaces: new[] { "Blog.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Blog.Web.Controllers" }
            );
        }
    }
}