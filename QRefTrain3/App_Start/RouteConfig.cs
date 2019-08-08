﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QRefTrain3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Profile",
                url: "Profile/{action}",
                defaults: new { controller = "Profile", action = "DisplayProfile" }
            );

            routes.MapRoute(
                name: "SeeProfile",
                url: "Profile/{action}/{id}",
                defaults: new { controller = "Profile", action = "DisplayProfile" }
            );

            routes.MapRoute(
                name: "ConfirmMail",
                url: "Login/ConfirmEmail",
                defaults: new { controller = "Login", action = "ConfirmEmail" }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login/{action}",
                defaults: new { controller = "Login", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin",
                url: "Admin/{action}",
                defaults: new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin2",
                url: "Admin",
                defaults: new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                name: "QuestionSuite",
                url: "QuestionSuite",
                defaults: new { controller = "QuestionSuite", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Homepage" }
            );


        }
    }
}
