using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace Blog.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static bool IsAction(this UrlHelper url, string action, string controller, string area = "", object routeValues = null)
        {
            if (routeValues != null)
            {
                var routes = new RouteValueDictionary(routeValues);
                foreach (var item in routes)
                {
                    if (url.RequestContext.RouteData.Values.ContainsKey(item.Key) && item.Value != null)
                    {
                        if (url.RequestContext.RouteData.Values[item.Key] as string != item.Value as string)
                        {
                            return false;
                        }
                    }
                }
            }

            return (
                       (action.IsEmpty() || url.RequestContext.RouteData.Values["action"].ToString() == action)
                       && ( area.IsEmpty() || url.RequestContext.RouteData.Values["area"].ToString() == area)
                       && url.RequestContext.RouteData.Values["controller"].ToString() == controller);
        }
    }
}