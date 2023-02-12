using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanVeMayBay
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                 name: "lien-he",
                 url: "lien-he",
                 defaults: new { controller = "Site", action = "lienHe", id = UrlParameter.Optional }
             );
          routes.MapRoute(
          name: "myaccout",
          url: "tai-khoan",
          defaults: new { controller = "Customer", action = "Myaccount", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "chi-tiet-order",
              url: "chi-tiet-order/{id}",
              defaults: new { controller = "Customer", action = "orderDetailCus", id = UrlParameter.Optional }
          );
              routes.MapRoute(
              name: "Đăng nhập",
              url: "dang-nhap",
              defaults: new { controller = "Customer", action = "login", id = UrlParameter.Optional }
          );
              routes.MapRoute(
              name: "dăng ký",
              url: "dang-ky",
              defaults: new { controller = "Customer", action = "register", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "chu-tiet-bv",
               url: "chi-tiet-bai-viet/{slug}",
               defaults: new { controller = "Site", action = "PostDetal", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                 name: "tim-kiem-bai-viet",
                 url: "tim-kiem-bai-viet",
                 defaults: new { controller = "Site", action = "postSearch", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                 name: "ctin-tuc",
                 url: "tin-tuc/{slug}",
                 defaults: new { controller = "Site", action = "postOftoPic", id = UrlParameter.Optional }
             );
            routes.MapRoute(
               name: "chi tiet chuyen bay",
               url: "chuyen-bay/{id}",
               defaults: new { controller = "Site", action = "flightDetail", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "chuyen bay trong ngay",
               url: "chuyen-bay-trong-ngay",
               defaults: new { controller = "Home", action = "flightOfday", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "chuyen bay",
                url: "all-chuyen-bay",
                defaults: new { controller = "Site", action = "AllChuyenBay", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
          
        }
    }
}
