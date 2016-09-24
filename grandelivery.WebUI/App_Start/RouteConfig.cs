using System.Web.Mvc;
using System.Web.Routing;

namespace grandelivery.WebUI
{
    public class RouteConfig
    {
        private static readonly string[] _defNamespace = new string[] { "grandelivery.Controllers" };

        public static void PreRouteAction(RouteCollection routes)
        {
            routes.MapRoute(
                name: null,
                url: "Clients",
                defaults: new { controller = "Clients", action = "List", area = "" },
                namespaces: _defNamespace
            );
        }

        public static void PostRouteAction(RouteCollection routes)
        {

        }
    }
}