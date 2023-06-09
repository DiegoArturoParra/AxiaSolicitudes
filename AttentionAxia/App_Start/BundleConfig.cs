﻿using System.Web.Optimization;

namespace AttentionAxia
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/adminlte/css").Include(
                    "~/adminlte/css/adminlte.min.css",
                    "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                    "~/adminlte/plugins/toastr/toastr.min.css",
                    "~/adminlte/plugins/ion-rangeslider/css/ion.rangeSlider.min.css",
                    "~/adminlte/plugins/daterangepicker/daterangepicker.css"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
                     "~/adminlte/js/adminlte.min.js",
                     "~/adminlte/plugins/moment/moment.min.js",
                     "~/adminlte/plugins/toastr/toastr.min.js",
                     "~/adminlte/plugins/ion-rangeslider/js/ion.rangeSlider.min.js",
                     "~/adminlte/plugins/daterangepicker/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/Alertify").Include(
                     "~/Scripts/alertify.js"
                   ));

            bundles.Add(new StyleBundle("~/alertifyjs/css").Include(
                     "~/Content/alertifyjs/alertify.min.css",
                      "~/Content/alertifyjs/themes/default.min.css"
                     ));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                               "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
        }
    }
}
