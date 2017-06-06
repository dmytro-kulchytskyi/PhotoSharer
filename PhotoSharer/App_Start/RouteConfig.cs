using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoSharer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
             name: "Details",
                url: "group-{id}-{link}",
                defaults: new
                {
                    controller = "Groups",
                    action = "Details",
                },
                namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
            );

            routes.MapRoute(
            name: "MyPhotoStreams",
               url: "group-{id}-{link}/my-streams",
               defaults: new
               {
                   controller = "Groups",
                   action = "MyPhotoStreams",
               },
               namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
           );

            routes.MapRoute(
             name: "MyGroups",
                url: "MyGroups",
                defaults: new
                {
                    controller = "Groups",
                    action = "MyGroups",
                },
                namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
            );

            routes.MapRoute(
             name: "OwnGroups",
                url: "OwnGroups",
                defaults: new
                {
                    controller = "Groups",
                    action = "OwnGroups",
                },
                namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Groups",
                    action = "MyGroups",
                    id = UrlParameter.Optional,
                },
                namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
            );

            routes.MapRoute(
               name: "404",
               url: "{*url}",
               defaults: new
               {
                   controller = "System",
                   action = "Http404",
               },
               namespaces: new string[] { "PhotoSharer.MVC.Controllers" }
           );
        }
    }
}
