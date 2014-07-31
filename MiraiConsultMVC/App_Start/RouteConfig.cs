using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ServiceModel.Activation;

namespace MiraiConsultMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Services/UserService.svc/{*pathInfo}");
            routes.IgnoreRoute("Content/image/DoctorPhotos/{*pathInfo}");

           

            //routes.MapRoute(
            //    name: "Questions",
            //    url: "{Question}/{questionId}",
            //    defaults: new { controller = "Patients", action = "QuestionDetails" },
            //    constraints: new { controller = "Patients", action = "QuestionDetails" }

            //);

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
           );
        }
    }
}