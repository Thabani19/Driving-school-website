using System.Web.Http;
using System.Web.Http.Cors;

namespace DrivingSchoolLandingPage.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            config.EnableSystemDiagnosticsTracing();

            // Enable attribute routing - MUST come first
            config.MapHttpAttributeRoutes();

            // Fallback default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
