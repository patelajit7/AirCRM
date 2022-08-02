#region using statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
#endregion

namespace TravelCRM.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/theme").Include(
                    "~/Content/css/font-awesome-all.min.css",
                    "~/Content/css/adminlte.min.css", /*BootStrap Already Add in this File */
                    "~/Content/css/sweetalert2.min"));  
            bundles.Add(new StyleBundle("~/Content/custom").Include(
                    "~/Content/css/styles.css"));
            bundles.Add(new ScriptBundle("~/scripts/theme").Include(
                    "~/Content/js/jquery-3.6.0.min.js",
                    "~/Content/js/bootstrap.bundle.min.js",
                     "~/Content/js/adminlte.js",
                     "~/Content/js/sweetalert2.all.min.js"));
            bundles.Add(new ScriptBundle("~/scripts/custom").Include(
                    "~/Content/js/main.js"));
            bundles.Add(new ScriptBundle("~/scripts/booking").Include(
                    "~/Content/js/booking.js"));
            bundles.Add(new ScriptBundle("~/scripts/validate").Include(
                    "~/Content/js/jquery.validate.min.js",
                    "~/Content/js/jquery.validate.unobtrusive.min.js"
                    ));
            bundles.Add(new ScriptBundle("~/scripts/chartjs").Include(
                    "~/Content/js/Chart.min.js"));

        }
    }
}









