using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using CarService.Data;
using CarService.Data.Migrations;
using CarService.Web.App_Start;

namespace CarService.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var languages = Request.UserLanguages;
            if (languages != null && languages.Length > 0)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(languages[0].Trim());
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            }
        }
        protected void Application_Start()
        {
            Database.SetInitializer<CarServiceDbContext>(new MigrateDatabaseToLatestVersion<CarServiceDbContext, Configuration>());

            ViewEnginesConfig.RegisterViewEngines();
            AutoMapperConfig.RegisterMappings();
            ModelBindersConfig.RegisterBindings();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
