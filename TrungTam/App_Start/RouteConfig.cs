using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrungTam
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BANG_LUONG",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "BANG_LUONG", action = "Index", id = UrlParameter.Optional }
            );
           // routes.MapRoute(
           //    name: "Details",
           //    url: "Admin/{controller}/{action}/{id}",
           //    defaults: new { controller = "CONG_NO", action = "Details", id = UrlParameter.Optional }
           //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
