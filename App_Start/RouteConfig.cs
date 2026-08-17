using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DrivingSchoolLandingPage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            // Ignore static file extensions
            routes.IgnoreRoute("{*allhtml}", new { allhtml = @".*\.html$" });
            routes.IgnoreRoute("{*allcss}", new { allcss = @".*\.css$" });
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js$" });
            routes.IgnoreRoute("{*allpng}", new { allpng = @".*\.png$" });
            routes.IgnoreRoute("{*alljpg}", new { alljpg = @".*\.jpg$" });
            routes.IgnoreRoute("{*allico}", new { allico = @".*\.ico$" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
