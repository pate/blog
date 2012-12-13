using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Blog.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        //public static MvcHtmlString ActiveActionLinkListItem(this HtmlHelper htmlHelper, object routeValues = null, object htmlAttributes = null)
        //{
        //    if (routeValues == null)
        //        throw new ArgumentException("routeValues dictionary is null");
            
        //    var routes = new RouteValueDictionary(routeValues);
        //    var isActive = routes.All(item => htmlHelper.ViewContext.RouteData.Values[item.Key] == item.Value);

        //    var 

        //    htmlHelper.ActionLink()

        //    //if ((actionName == "" || html.ViewContext.RouteData.Values["action"].ToString() == actionName) &&
        //    //        html.ViewContext.RouteData.Values["controller"].ToString() == controlName)
        //    //    return html.ActionLink(linkText, actionName, new { controller = controlName, id = actionId }, new { @class = "selected", @title = linkTitle });


        //    //return html.ActionLink(linkText, actionName, new { controller = controlName, id = actionId }, new { @title = linkTitle });
        //}

        /// <summary>
        /// Returns a checkbox for each of the provided <paramref name="items"/>.
        /// </summary>
        public static MvcHtmlString CheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
                                                                    Expression<Func<TModel, TValue>> expression,
                                                                    IEnumerable<SelectListItem> items,
                                                                    object htmlAttributes = null)
        {
            var listName = ExpressionHelper.GetExpressionText(expression);
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            items = GetCheckboxListWithDefaultValues(metaData.Model, items);
            return htmlHelper.CheckBoxList(listName, items, htmlAttributes);
        }

        /// <summary>
        /// Returns a checkbox for each of the provided <paramref name="items"/>.
        /// </summary>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string listName,
                                                 IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            var container = new TagBuilder("div");
            foreach (var item in items)
            {
                var label = new TagBuilder("label");
                label.MergeAttribute("class", "checkbox"); // default class
                label.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

                var cb = new TagBuilder("input");
                cb.MergeAttribute("type", "checkbox");
                cb.MergeAttribute("name", listName);
                cb.MergeAttribute("value", item.Value ?? item.Text);
                if (item.Selected)
                    cb.MergeAttribute("checked", "checked");

                label.InnerHtml = cb.ToString(TagRenderMode.SelfClosing) + item.Text;

                container.InnerHtml += label.ToString();
            }

            return new MvcHtmlString(container.ToString());
        }


        private static IEnumerable<SelectListItem> GetCheckboxListWithDefaultValues(object defaultValues,
                                                                                    IEnumerable<SelectListItem>
                                                                                        selectList)
        {
            var defaultValuesList = defaultValues as IEnumerable;

            if (defaultValuesList == null)
                return selectList;

            IEnumerable<string> values = from object value in defaultValuesList
                                         select Convert.ToString(value, CultureInfo.CurrentCulture);

            var selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
            var newSelectList = new List<SelectListItem>();

            foreach (var item in selectList)
            {
                item.Selected = (item.Value != null)
                                    ? selectedValues.Contains(item.Value)
                                    : selectedValues.Contains(item.Text);
                newSelectList.Add(item);

            }

            return newSelectList;
        }

        public static MvcHtmlString ActiveActionLinkHelper(this HtmlHelper html, string linkText, string actionName, string controlName, string linkTitle, string activeClassName, string actionId)
        {
            if ((actionName == "" || html.ViewContext.RouteData.Values["action"].ToString() == actionName) &&
                    html.ViewContext.RouteData.Values["controller"].ToString() == controlName)
                return html.ActionLink(linkText, actionName, new { controller = controlName, id = actionId }, new { @class = "selected", @title = linkTitle });

            return html.ActionLink(linkText, actionName, new { controller = controlName, id = actionId }, new { @title = linkTitle });
        }
    }
}