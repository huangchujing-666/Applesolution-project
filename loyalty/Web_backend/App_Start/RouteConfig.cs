using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Palmary.Loyalty.Web_backend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // for default startup
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Land", } //id = UrlParameter.Optional }, 
          
            );

            // for Home controller without action
            routes.MapRoute(
               name: "Home Default",
               url: "Home",
               defaults: new { controller = "Home", action = "Land", } //id = UrlParameter.Optional },
              
           );

            routes.MapRoute(
               name: "Module",
               url: "{controller}/{action}/{Module}",
               defaults: new { controller = "Home", action = "Login" },
               constraints: new { Module = @"^[^\d]\w+" }
             
            );

            routes.MapRoute(
               name: "Id",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Login", id = 0 },
               constraints: new { id = @"^\d+" }
            );

            routes.MapRoute(
                 name: "ModuleId",
                 url: "{controller}/{action}/{Module}/{Id}",
                 defaults: new { controller = "Home", action = "Login", id = 0 },
                 constraints: new { Module = @"^[^\d]\w+", Id = @"^\d+" }

            );
        }
    }
}