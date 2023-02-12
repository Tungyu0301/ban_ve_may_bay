using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace BanVeMayBay.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (Userlogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }

            if (session.AccessName.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (Userlogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            //neu chua dang nhap
            if (session==null)
            {
                RouteValueDictionary route = new RouteValueDictionary(new { Controller = "Auth", Action = "Login" });
                filterContext.Result = new RedirectToRouteResult(route);
                return;
            }
            // neu da dang nhap ma khong co quyen truy cap
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
            };
        }
        

    }
}