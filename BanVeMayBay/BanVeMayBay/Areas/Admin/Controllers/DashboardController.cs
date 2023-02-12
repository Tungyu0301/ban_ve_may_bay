using BanVeMayBay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult usersession()
        {
            var session = (Userlogin)Session[Common.CommonConstants.USER_SESSION];
            return View("_adminSession", session);
        }
        
    }
}