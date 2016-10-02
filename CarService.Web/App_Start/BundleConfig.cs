using System.Web;
using System.Web.Optimization;

namespace CarService.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/Custom/put-cursor-at-end.js",
                        "~/Scripts/Custom/show-hide-element.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/Custom/fix-jquery-validation.js"));

            //bundles.Add(new ScriptBundle("~/bundles/homeindex")
            //    .Include("~/Scripts/Custom/home-index.js"));

            bundles.Add(new ScriptBundle("~/bundles/timezone-offset")
                .Include("~/Scripts/Custom/set-timezone-cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalize")
                .Include("~/Scripts/globalize/globalize.js")
                .Include("~/Scripts/globalize/cultures/globalize.culture.bg-BG.js"));

            bundles.Add(new ScriptBundle("~/bundles/document-car-create-edit-functionality")
                .Include("~/Scripts/Custom/repair-document-create-edit.js")
                .Include("~/Scripts/Custom/car-modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui")
                .Include("~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ajax")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.darkly.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui-styles")
                .IncludeDirectory("~/Content/images", ".png")
                .Include("~/Content/jquery-ui.*"));
        }
    }
}
