using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.Web.UI;
using Benchmark_Instant_Reports_2.Infrastructure.IoC;
using StructureMap;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;






namespace Benchmark_Instant_Reports_2
{


    public class Global : System.Web.HttpApplication
    {


        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // StructureMap Setter Injection for repositories for each web page
            var application = (HttpApplication)sender;
            var page = application.Context.CurrentHandler as Page;
            if (page == null) return;
            ObjectFactory.BuildUp(page);
        }


        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // StructureMap - dispose of HTTP-scoped objects
            //var disposable = ObjectFactory.GetInstance<IRepoService>() as IDisposable;
            //if (disposable != null) disposable.Dispose();
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            //RouteTable.Routes.Add("HelpRoute", new Route("help", new   WebFormRouteHandler("Info.aspx")));

            // StructureMap
            StructureMapConfig.Configure();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }





    }
}
