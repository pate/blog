using Blog.Web;
using Blog.Web.Helpers;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SiteAuthorizeAttribute
        : AuthorizeAttribute
    {
        public string Task { get; set; }

        public SiteAuthorizeAttribute(params Roles[] roles)
        {
            foreach (var role in roles)
            {
                Roles += ", " + role.GetStringValue();
            }
            if (Roles.Length >= 2)
                Roles = Roles.Substring(2);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(Roles))
                {
                    string[] roleNames = Roles.Split(',');

                    foreach (string role in roleNames)
                    {
                        if (filterContext.HttpContext.User.IsInRole(role))
                        {
                            //base.OnAuthorization(filterContext);
                            base.OnAuthorization(filterContext);
                            return;
                        }
                    }

                    filterContext.Controller.FlashError("You must be assigned one of the following roles" +
                                                        (String.IsNullOrEmpty(Task) ? "" : (" to " + Task)) + ": " +
                                                        Roles);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    // all cool. Go on ahead.
                }

            }
            else
                filterContext.Result = new HttpUnauthorizedResult();
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }
    }
}