using Owin;
using System.Web.Http; 

namespace OWINSelfHost
{

    // The StartUp class is where configuration is set - particularly routing
    // Routing is now set via attributes in the controllers, so the StartUp config is not required.

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{fishId}",
                defaults: new { fishId = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

        }
    } 

}
