using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Voice
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "sound", action = "latest" }
            );      
                   
            routes.MapRoute(
                name: "Author",
                url: "{controller}/{action}/{name}/{audioTitle}",
                defaults: new { controller = "sound", action = "author", name = "lesha", audioTitle = UrlParameter.Optional }
            );
        }
    }
}
