using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClinicManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapRoute(
            //   name: "api",
            //   url: "{controller}",
            //   defaults: new { controller = "Values", action = "demo", id = UrlParameter.Optional },
            //   namespaces: new[] { "ClinicManagementSystem.Controllers" }
            //);

            //routes.MapRoute(
            //   name: "Contact",
            //   url: "contact",
            //   defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
            //   namespaces: new[] { "ClinicManagementSystem.Controllers" }
            //);

            //routes.MapRoute(
            //    name: "Cart",
            //    url: "cart",
            //    defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "ClinicManagementSystem.Controllers" }
            //);
            

            routes.MapRoute(
                name: "Add Medicine To Cart",
                url: "add-medicine",
                defaults: new { controller = "Cart", action = "AddMedicineItem", id = UrlParameter.Optional },
                namespaces: new[] { "ClinicManagementSystem.Controllers" }
            );
            routes.MapRoute(
               name: "Add Apparatus To Cart",
               url: "add-apparatus",
               defaults: new { controller = "Cart", action = "AddApparatusItem", id = UrlParameter.Optional },
               namespaces: new[] { "ClinicManagementSystem.Controllers" }
           );

            //routes.MapRoute(
            //    name: "Medicine",
            //    url: "{controller}/{id}",
            //    defaults: new { controller = "Medicine", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "ClinicManagementSystem.Controllers" }
            //);

            //routes.MapRoute(
            //    name: "ScientificApparatus",
            //    url: "{controller}/{id}",
            //    defaults: new { controller = "ScientificApparatus", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "ClinicManagementSystem.Controllers" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ClinicManagementSystem.Controllers" }
                
            );
        }
    }
}
