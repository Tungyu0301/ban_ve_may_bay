
using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVeMayBay.Common;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        // GET: Admin/Auth
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        public ActionResult login()
        {
            return View("_login");
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            String Username = fc["username"];
            string Pass = fc["password"];
            var user_account = db.users.Where(m => m.access != 1 && m.status == 1 && (m.username == Username));
            var userC = db.users.Where(m => m.username == Username && m.access == 1);
            if (userC.Count() != 0)
            {
                ViewBag.error = "Bạn không có quyền đăng nhập";
            }
            else
            {
                if (user_account.Count() == 0)
                {
                    ViewBag.error = "Sai Username ";
                }
                else
                {
                    var pass_account = db.users.Where(m => m.access != 1 && m.status == 1 && m.password == Pass);
                    if (pass_account.Count() == 0)
                    {
                        ViewBag.error = "Sai password";
                    }
                    else
                    {
                        var user = user_account.First();
                        role role = db.roles.Where(m => m.parentId == user.access).First();
                        var userSession = new Userlogin();
                        userSession.UserName = user.username;
                        userSession.UserID = user.ID;
                        userSession.GroupID = role.GropID;
                        userSession.AccessName = role.accessName;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        var i = Session["SESSION_CREDENTIALS"];
                        Session["Admin_id"] = user.ID;
                        Session["Admin_user"] = user.username;
                        Session["Admin_fullname"] = user.fullname;
                        Response.Redirect("~/Admin");
                    }
                }
            }
            ViewBag.sess = Session["Admin_id"];
            return View("_login");

        }

        public ActionResult logout()
        {
            Session["Admin_id"] = "";
            Session["Admin_user"] = "";
            Response.Redirect("~/dang-nhap");
            return View();
        }
        public ActionResult EditUser()
        {
            int id = 1;
            id = int.Parse(Session["Admin_id"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user muser = db.users.Find(id);
            if (muser == null)
            {
                return HttpNotFound();
            }
            ViewBag.role = db.roles.ToList();
            return View(muser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(user muser)
        {
            if (ModelState.IsValid)
            {
                muser.img = "ádasd";
                muser.updated_at = DateTime.Now;
                muser.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(muser).State = EntityState.Modified;
                db.SaveChanges();
                Message.set_flash("Update thành công", "success");
                return RedirectToAction("EditUser");
            }
            return View(muser);
        }
    }
}