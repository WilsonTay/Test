﻿using System.Web.Mvc;
using System.Web.Routing;

namespace DealsWhat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "GeneralTerms",
              url: "general-terms",
              defaults: new { controller = "GeneralTerms", action = "Index" }
          );

            routes.MapRoute(
              name: "Deal",
              url: "deal/{id}",
              defaults: new { controller = "Deal", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

          
        }
    }
}