using grandelivery.WebUI.Infrastructure;
using SX.WebCore.MvcApplication;
using System;

namespace grandelivery.WebUI
{
    public class MvcApplication : SxMvcApplication
    {
        public static string[] CustomRoles
        {
            get
            {
                return new string[] { "customer", "carrier" };
            }
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs
            {
                GetDbContextInstance = () => { return new DbContext(); },
                WebApiConfigRegister = WebApiConfig.Register,
                MapperConfigurationExpression = AutoMapperConfig.Register,

                //routes
                DefaultControllerNamespaces = new string[] { "grandelivery.Controllers" },
                PreRouteAction = RouteConfig.PreRouteAction,
                PostRouteAction = RouteConfig.PostRouteAction
            };

            base.Application_Start(sender, args);
        }
    }
}
