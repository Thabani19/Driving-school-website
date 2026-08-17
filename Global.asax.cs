using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using DrivingSchoolLandingPage.App_Start;

namespace DrivingSchoolLandingPage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            // Allow static files to be served directly
            string filePath = Request.PhysicalPath;
            if (System.IO.File.Exists(filePath))
            {
                // Let IIS handle static files
                return;
            }
        }
    }
}
