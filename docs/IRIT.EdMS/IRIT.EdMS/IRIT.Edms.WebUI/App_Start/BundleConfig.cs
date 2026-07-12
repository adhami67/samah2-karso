using System.Web;
using System.Web.Optimization;
using IRIT.Framework.Utility.Helpers;

namespace IRIT.EdMS.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/libs/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/kendo/2015.3.930/jquery.min.js",
                "~/Scripts/kendo/2015.3.930/kendo.all.min.js",
                "~/Scripts/kendo/2015.3.930/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo.modernizr.custom.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jqueryval-default.min.js")
                        .Include("~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap-dialog").Include(
            //"~/Scripts/libs/bootstrap-dialog.min.js",
            //"~/Scripts/libs/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-dialog").Include(
            "~/Scripts/libs/bootstrap-dialog.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/libs/bootstrap.min.js",
                "~/Scripts/libs/respond.min.js",
                "~/Scripts/libs/bootstrap-dialog.min.js",
                "~/Scripts/plugins/fileinput.min.js",
                "~/Scripts/plugins/jquery.noty.packaged.min.js",
                "~/Scripts/libs/site.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            "~/Scripts/libs/modernizr-*"
           ));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                     "~/Scripts/morris/morris.js",
                     "~/Scripts/morris/morris-data.js",
                     "~/Scripts/morris/raphael.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                     "~/Scripts/flot/excanvas.js",
                     "~/Scripts/flot/flot-data.js",
                     "~/Scripts/flot/jquery.flot.js",
                     "~/Scripts/flot/jquery.flot.pie.js",
                     "~/Scripts/flot/jquery.flot.resize.js",
                     "~/Scripts/flot/jquery.flot.tooltip.js"));

            bundles.Add(new ScriptBundle("~/bundles/Calendar").Include(
                        "~/Scripts/libs/jquery-ui-{version}.js",
                        "~/Scripts/libs/bootstrap-modal.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/adminJs").Include(
            //   "~/Scripts/libs/bootstrap.min.js",
            //   "~/Scripts/libs/respond.min.js",
            //   "~/Scripts/plugins/jquery.noty.packaged.min.js",
            //   "~/Scripts/plugins/fileinput.min.js",
            //   "~/Scripts/libs/site.min.js"
            //   ));
            #endregion

            #region Theme

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Default).Include("~/Content/site.css",
                "~/Content/kendo/2015.3.930/kendo.common.css",
                "~/Content/kendo/2015.3.930/kendo.dataviz.css",
                "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Default + ".css",
                "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Default + ".css",
                "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Black).Include("~/Content/site.css",
                "~/Content/kendo/2015.3.930/kendo.common.css",
                "~/Content/kendo/2015.3.930/kendo.dataviz.css",
                "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Black + ".css",
                "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Black + ".css",
                "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.BlueOpal).Include("~/Content/site.css",
                "~/Content/kendo/2015.3.930/kendo.common.css",
                "~/Content/kendo/2015.3.930/kendo.dataviz.css",
                "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.BlueOpal + ".css",
                "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.BlueOpal + ".css",
                "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Bootstrap).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Bootstrap + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Bootstrap + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Default).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Default + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Default + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Flat).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Flat + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Flat + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.HighContrast).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.HighContrast + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.HighContrast + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Metro).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Metro + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Metro + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.MetroBlack).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.MetroBlack + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.MetroBlack + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Moonlight).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Moonlight + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Moonlight + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Silver).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Silver + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Silver + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/" + ThemeHelper.Uniform).Include("~/Content/site.css",
               "~/Content/kendo/2015.3.930/kendo.common.css",
               "~/Content/kendo/2015.3.930/kendo.dataviz.css",
               "~/Content/kendo/2015.3.930/kendo." + ThemeHelper.Uniform + ".css",
               "~/Content/kendo/2015.3.930/kendo.dataviz." + ThemeHelper.Uniform + ".css",
               "~/Content/kendo/2015.3.930/kendo.rtl.css"));

            #endregion

            #region Contents
            bundles.Add(new StyleBundle("~/Content/css-rtl").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl.css",
                      "~/Content/sb-admin.css",
                      "~/fonts/font-awesome/css/font-awesome.css",
                      "~/fonts/fonts/font-awesome.css",
                      "~/Content/sb-admin-rtl.css",
                      "~/Content/site-rtl.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-dialog").Include(
                      "~/Content/bootstrap-dialog.min.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
            "~/Content/kendo/2015.3.930/kendo.common.min.css",
            "~/Content/kendo/2015.3.930/kendo.default.min.css"));

            bundles.Add(new StyleBundle("~/Content/Calendar").Include(
                       "~/Content/Calendar/base/jquery.ui.core.css",
                       "~/Content/Calendar/base/jquery.ui.resizable.css",
                       "~/Content/Calendar/base/jquery.ui.selectable.css",
                       "~/Content/Calendar/base/jquery.ui.accordion.css",
                       "~/Content/Calendar/base/jquery.ui.autocomplete.css",
                       "~/Content/Calendar/base/jquery.ui.button.css",
                       "~/Content/Calendar/base/jquery.ui.dialog.css",
                       "~/Content/Calendar/base/jquery.ui.slider.css",
                       "~/Content/Calendar/base/jquery.ui.tabs.css",
                       "~/Content/Calendar/base/jquery.ui.datepicker.css",
                       "~/Content/Calendar/base/jquery.ui.progressbar.css",
                       "~/Content/Calendar/base/jquery.ui.theme.css"));

            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}
