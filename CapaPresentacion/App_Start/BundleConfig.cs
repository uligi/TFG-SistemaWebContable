using System.Web;
using System.Web.Optimization;

namespace CapaPresentacion
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/js/libs/popper/popper.js",
                "~/Scripts/js/bootstrap.js",
                "~/Scripts/js/sidenav.js",
                "~/Scripts/js/layout-helpers.js",
                "~/Scripts/js/material-ripple.js",
                "~/Scripts/js/libs/perfect-scrollbar/perfect-scrollbar.js",
                "~/Scripts/js/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
     "~/Scripts/js/dataTables/datatables.js", "~/Scripts/js/dataTables/datatables.min.js"
 ));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
    "~/Scripts/js/libs/eve/eve.js",
    "~/Scripts/js/libs/raphael/raphael.js",
    "~/Scripts/js/libs/morris/morris.js"
));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/assets/fonts/fontawesome.css",
                "~/Content/assets/fonts/ionicons.css",
                "~/Content/assets/fonts/linearicons.css",
                "~/Content/assets/fonts/open-iconic.css",
                "~/Content/assets/fonts/pe-icon-7-stroke.css",
                "~/Content/assets/fonts/feather.css",
                "~/Content/bootstrap-material.css",
                "~/Content/shreerang-material.css",
                "~/Content/uikit.css",
                "~/Content/Site.css",
                "~/Content/assets/pages/authentication.css"));

            bundles.Add(new Bundle("~/bundles/datatables").Include(
     "~/Scripts/js/dataTables/datatables.min.js"
 ));

            bundles.Add(new Bundle("~/Content/datatables").Include(
                "~/Scripts/js/dataTables/datatables.min.css"
            ));
        }
    }
}