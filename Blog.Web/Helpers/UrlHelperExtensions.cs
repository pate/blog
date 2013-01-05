using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static bool IsAction(this UrlHelper url, string action, string controller, object routeValues = null)
        {
            if (routeValues != null)
            {
                var routes = new RouteValueDictionary(routeValues);
                foreach (var item in routes)
                {
                    if (url.RequestContext.RouteData.Values.ContainsKey(item.Key))
                    {
                        if (url.RequestContext.RouteData.Values[item.Key] != item.Value)
                            return false;
                    }
                }
            }

            return (
                       (action == null || url.RequestContext.RouteData.Values["action"].ToString() == action)
                       && url.RequestContext.RouteData.Values["controller"].ToString() == controller);
        }
    }
}