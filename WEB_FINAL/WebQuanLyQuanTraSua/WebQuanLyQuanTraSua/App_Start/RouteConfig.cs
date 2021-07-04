using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebQuanLyQuanTraSua
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "thuc-don",
                url: "thuc-don",
                defaults: new { controller = "Home", action = "MENUKH", id = UrlParameter.Optional },
                namespaces: new[] { "WebQuanLyQuanTraSua.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] { "WebQuanLyQuanTraSua.Controllers" }
            );

        }
    }
}
