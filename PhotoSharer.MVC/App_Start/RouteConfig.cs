using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoSharer.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LangGroup",
                url: "{language}/Group/{url}",
                constraints: new
                {
                    language = @".+-.+"
                },
                defaults: new
                {
                    controller = "Groups",
                    action = "Group",
                }
            );

            routes.MapRoute(
                name: "LangDefault",
                url: "{language}/{controller}/{action}/{id}",
                constraints: new
                {
                    language = @".+-.+"
                },
                defaults: new
                {
                    controller = "Groups",
                    action = "Index",
                    language = "en-en",
                    id = UrlParameter.Optional,
                }
            );

            routes.MapRoute(
             name: "Group",
                url: "Group/{url}",
                defaults: new
                {
                    controller = "Groups",
                    action = "Group",
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Groups",
                    action = "Index",
                    id = UrlParameter.Optional,
                }
            );
        }
    }
}
