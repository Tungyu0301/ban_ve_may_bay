using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["Admin_id"].Equals(""))
            {
                RouteValueDictionary route = new RouteValueDictionary(new { Controller = "Auth", Action = "Login" });
                filterContext.Result = new RedirectToRouteResult(route);
                return;
            }
        }
    }
}