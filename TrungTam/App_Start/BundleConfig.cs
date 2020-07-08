using System.Web;
using System.Web.Optimization;

namespace TrungTam
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
              "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                       "~/Asset/admin/js/main.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/PagedList.css",                    
                      "~/Content/jquery-ui.css",
                      "~/Content/style.css",
                      "~/Content/style1_css.css",
                      "~/Content/style2_font.css"
                      ));
            var fontCDNPath = "https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i";
            bundles.Add(new StyleBundle("~/bundles/fonts", fontCDNPath).Include(

                         ));
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                         "~/Scripts/sb-admin-2.js",
                      "~/Scripts/boot/bootbox.js",
                      "~/Scripts/boot/bootbox.locales.js"));

        }
    }
}
