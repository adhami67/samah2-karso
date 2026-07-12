using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using CaptchaMvc.Infrastructure;
using ElmahEFLogger.CustomElmahLogger;
using IRIT.Framework.Utility.Helpers.Json;
using System.Linq;
using System.Web.Mvc;
using IRIT.Framework.DataAccess.Context;
using IRIT.Framework.DataAccess.Migrations;

namespace IRIT.EdMS.WebUI
{
    public static class ApplicationStart
    {
        public static void Config()
        {
            // disable response header for protection  attak
            MvcHandler.DisableMvcResponseHeader = true;

            // change captcha provider for using cookie
            CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();

            var defaultJsonFactory = ValueProviderFactories.Factories
                .OfType<JsonValueProviderFactory>().FirstOrDefault();
            var index = ValueProviderFactories.Factories.IndexOf(defaultJsonFactory);
            ValueProviderFactories.Factories.Remove(defaultJsonFactory);
            ValueProviderFactories.Factories.Insert(index, new JsonNetValueProviderFactory());

            Database.SetInitializer<ApplicationDbContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            //ad interception for logg EF errors
            DbInterception.Add(new ElmahEfInterceptor());

            AutoMapperWebConfiguration.Configure();
        }       
    }
}