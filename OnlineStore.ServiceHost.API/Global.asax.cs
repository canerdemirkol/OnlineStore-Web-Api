using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using OnlineStore.Core;

namespace OnlineStore.ServiceHost.API
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //AreaRegistration.RegisterAllAreas();
            //   RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            DependencyContainer.Bootstrap();

            //  HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }
    }
}