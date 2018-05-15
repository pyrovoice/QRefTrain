using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QRefTrain3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //To remove in production, is used to update db when there are changes in the code
            Database.SetInitializer<QuestionsContext>(new DropCreateDatabaseIfModelChanges<QuestionsContext>());
            new InitApplicationClass().InitApplication();
        }
    }
}
