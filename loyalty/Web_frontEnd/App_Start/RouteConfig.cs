using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web_frontend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SingleDefault",
                url: "{controller}",
                defaults: new { LangCode = "tc", controller = "Loyalty", action = "Index" });

            routes.MapRoute(
                name: "MatchLang",
                url: "{controller}/{LangCode}/{action}",
                defaults: new { LangCode = "tc", controller = "Loyalty", action = "Index" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Loyalty", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}