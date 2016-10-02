using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarService.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Parts",
                url: "Parts/{action}/{id}",
                defaults: new { controller = "Parts", action = "Search", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Cars",
                url: "Cars/{action}/{id}",
                defaults: new { controller = "Cars", action = "Search", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Documents",
                url: "RepairDocuments/{action}/{id}",
                defaults: new { controller = "RepairDocuments", action = "Search", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
