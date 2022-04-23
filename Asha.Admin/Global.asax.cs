using AshaAdmin;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Asha.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string LogFilePath = (string)ConfigurationManager.AppSettings["LogFilePath"];
            log4net.GlobalContext.Properties["LogFilePath"] = LogFilePath;
            XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            AutoMapperConfig.Configure();
        }
    }
}
