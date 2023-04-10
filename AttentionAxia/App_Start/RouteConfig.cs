using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AttentionAxia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Error/Index");
            routes.IgnoreRoute("Error/Status404");
            routes.IgnoreRoute("Solicitud/Index");

            routes.MapRoute(
            name: "Page-Solicitudes",
            url: "Solicitudes",
            defaults: new { controller = "Solicitud", action = "Index" }
            );

            routes.MapRoute(
            name: "Page-Error",
            url: "Internal-Server-Error",
            defaults: new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
            name: "Page-NotFound",
            url: "NotFound",
            defaults: new { controller = "Error", action = "Status404" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
