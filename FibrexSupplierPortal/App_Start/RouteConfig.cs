using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing; 
using Microsoft.AspNet.FriendlyUrls;
using System.Web.Http;

namespace FibrexSupplierPortal
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings);
        }       
    }
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(); 
        }
    }
}
