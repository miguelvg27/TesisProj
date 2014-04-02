using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TesisProj.Models.Storage;
using WebMatrix.WebData;
using Devtalk.EF.CodeFirst;
using TesisProj.Controllers;

namespace TesisProj
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        static bool IsLocalDb = System.Configuration.ConfigurationManager.AppSettings["IsLocalDb"].ToString().Equals("True");
        static bool InitDb = System.Configuration.ConfigurationManager.AppSettings["DbStage"].ToString().Equals("Init");
        static bool SeedDb = System.Configuration.ConfigurationManager.AppSettings["DbStage"].ToString().Equals("Seed");

        public static string ConnectionString = IsLocalDb ? "TProjContextLocal" : "TProjContextAppHb";

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FailWhale",
                url: "FailWhale/{action}/{id}",
                defaults: new {
                    controller = "Error",
                    action = "FailWhale",
                    id = UrlParameter.Optional
                }
            );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Home_default",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin_default",
                url: "Admin/{action}/{id}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (InitDb)
            {
                Database.SetInitializer<TProjContext>(new DontDropDbJustCreateTablesIfModelChanged<TProjContext>());            
            }
            else
            {
                Database.SetInitializer<TProjContext>(new TProjInitializer());
            }

            using (TProjContext context = new TProjContext())
            {
                if (SeedDb) context.Seed();
                context.ColaboradoresRequester.All();
            }
        }

    }
}
