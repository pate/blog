using System.Web.Optimization;
using Blog.Web.Infrastructure;

namespace Blog.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/scripts/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/all")
                            .Include("~/Scripts/jquery-{version}.js")
                            .Include("~/Scripts/bootstrap.js")
                            .Include("~/Scripts/jquery.smooth-scroll.js")
                            .Include("~/Scripts/select2.js")
                            .Include("~/Scripts/blog.js")
                );

            var css = new StyleBundle("~/bundles/styles/all").Include(
                "~/Content/less/site.less");

            var lessMinifyTransformer = new LessMinify();

            css.Transforms.Add(lessMinifyTransformer);
            bundles.Add(css);

            var adminCss = new StyleBundle("~/bundles/styles/admin").Include(
                "~/Areas/Admin/Content/admin.less");

            adminCss.Transforms.Add(lessMinifyTransformer);
            bundles.Add(adminCss);
        }
    }
}