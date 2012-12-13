using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using PagedList.Mvc;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace Blog.Web.Helpers
{
    public static class PagingHelpers
    {
        /// <summary>
        /// Builds a RouteValueDictionary that combines the request route values, the querystring parameters,
        /// and the passed newRouteValues. Values from newRouteValues override request route values and querystring
        /// parameters having the same key.
        /// </summary>
        public static RouteValueDictionary GetCombinedRouteValues(this ViewContext viewContext, object newRouteValues)
        {
            var combinedRouteValues = new RouteValueDictionary(viewContext.RouteData.Values);

            NameValueCollection queryString = viewContext.RequestContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.AllKeys.Where(key => key != null))
                combinedRouteValues[key] = queryString[key];

            if (newRouteValues != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newRouteValues))
                    combinedRouteValues[descriptor.Name] = descriptor.GetValue(newRouteValues);
            }

            return combinedRouteValues;
        }

        public static MvcHtmlString Pager(this HtmlHelper html, IPagedList list, string action, string controller)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            return html.PagedListPager(list,
                                       page =>
                                       urlHelper.Action(action, controller,
                                                        html.ViewContext.GetCombinedRouteValues(new {page = page})),
                                       PagedListRenderOptions.DefaultPlusFirstAndLast);
        }
    }
}