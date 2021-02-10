using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Restaurant
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Restaurant", "Restaurant/{action}/{name}", new { controller = "Restaurant", action = "Index", name = UrlParameter.Optional }, new[] { "Restaurant.Controllers" });

            routes.MapRoute("MealOrder", "MealOrder/{action}/{id}", new { controller = "MealOrder", action = "Index", id = UrlParameter.Optional }, new[] { "Restaurant.Controllers" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
