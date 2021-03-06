using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Product Category",
            url: "product/{metatitile}-{cateId}",
            defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
            namespaces: new[] { "WebDemo.Controllers" }
        );

            routes.MapRoute(
            name: "Product Detail",
            url: "detail/{metatitile}-{id}",
            defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
            namespaces: new[] { "WebDemo.Controllers" }
        );
            routes.MapRoute(
            name: "Add Cart",
            url: "add-to-cart",
            defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
            namespaces: new[] { "WebDemo.Controllers" }
        );

            routes.MapRoute(
            name: "Cart",
            url: "cart-product",
            defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "WebDemo.Controllers" }
          );

            routes.MapRoute(
          name: "Login",
          url: "login",
          defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
          namespaces: new[] { "WebDemo.Controllers" }
        );

            routes.MapRoute(
         name: "Search",
         url: "search",
         defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
         namespaces: new[] { "WebDemo.Controllers" }
       );

            routes.MapRoute(
          name: "Register",
          url: "register",
          defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
          namespaces: new[] { "WebDemo.Controllers" }
        );
            routes.MapRoute(
                name: "Payment",
                url: "payment",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "WebDemo.Controllers" }
              );
            routes.MapRoute(
             name: "PaymentWithPayPal",
             url: "paymentwithpaypal",
             defaults: new { controller = "Cart", action = "PaymentWithPayPal", id = UrlParameter.Optional },
             namespaces: new[] { "WebDemo.Controllers" }
           );

            routes.MapRoute(
           name: "Payment Success",
           url: "success",
           defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
           namespaces: new[] { "WebDemo.Controllers" }
         );
          routes.MapRoute(
          name: "Introduction",
          url: "intro",
          defaults: new { controller = "Introduction", action = "Index", id = UrlParameter.Optional },
          namespaces: new[] { "WebDemo.Controllers" }
          );

          routes.MapRoute(
          name: "Contact",
          url: "contact",
          defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
          namespaces: new[] { "WebDemo.Controllers" }
          );

         routes.MapRoute(
         name: "News",
         url: "new",
         defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional },
         namespaces: new[] { "WebDemo.Controllers" }
         );


            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "WebDemo.Controllers" }
            );



        }
    }
}
