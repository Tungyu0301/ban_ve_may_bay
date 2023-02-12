using BanVeMayBay.Common;
using BanVeMayBay.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BanVeMayBay.Areas.Admin.Controllers
{
  
    public class UserController : BaseController
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        // GET: Admin/User
        public ActionResult Index()
        {
            var list = db.users.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user muser = db.users.Find(id);
            if (muser == null)
            {
                return HttpNotFound();
            }
            return View(muser);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        // GET: Admin/User/Create
        public ActionResult Create()
        {
            ViewBag.role = db.roles.ToList();
            return View();
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user muser, FormCollection data)
        {
            if (ModelState.IsValid)
            {
                string password1 = data["password1"];
                string password2 = data["password2"];
                string username = muser.username;
                var Luser = db.users.Where(m => m.status == 1 && m.username == username);
                if (password1!=password2) {ViewBag.error = "Password không trùng"; }
                if (Luser.Count()>0) { ViewBag.error1 = "Username đã tồn tại"; }
                else
                {
                    string pass = Mystring.ToMD5(password1);
                    muser.img = "ádasd";
                    muser.password = pass;
                    muser.address = "";
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = int.Parse(Session["Admin_id"].ToString());
                    muser.updated_by = int.Parse(Session["Admin_id"].ToString());
                    db.users.Add(muser);
                    db.SaveChanges();
                    Message.set_flash("Thêm user thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            return View(muser);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        // GET: Admin/User/Edit/5
        public ActionResult Edit(int? id)
        {
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
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(user muser)
        {
            if (ModelState.IsValid)
            {
                    muser.img = "ádasd";               
                    muser.updated_at = DateTime.Now;
                    muser.updated_by = int.Parse(Session["Admin_id"].ToString());
                    db.Entry(muser).State = EntityState.Modified;
                    db.SaveChanges();
                Message.set_flash("Update thành công", "success");
                return RedirectToAction("Index");
            }
            return View(muser);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        //status
        public ActionResult Status(int id)
        {
            user muser = db.users.Find(id);
            muser.status = (muser.status == 1) ? 2 : 1;
            muser.updated_at = DateTime.Now;
            muser.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(muser).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.users.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            user muser = db.users.Find(id);
            if (muser.ID == int.Parse(Session["Admin_id"].ToString()))
            {
                Message.set_flash("Not Delete Admin", "danger");
                return RedirectToAction("Index");
            }
            muser.status = 0;
            muser.updated_at = DateTime.Now;
            muser.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(muser).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Retrash(int id)
        {
            user muser = db.users.Find(id);
            muser.status = 2;
            muser.updated_at = DateTime.Now;
            muser.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(muser).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Phục hồi thành công", "success");
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            user muser = db.users.Find(id);
            if(muser.ID == int.Parse(Session["Admin_id"].ToString()))
            {
                Message.set_flash("Not Delete Admin", "danger");
                return RedirectToAction("trash");
            }
            db.users.Remove(muser);
            db.SaveChanges();
            Message.set_flash("Đã xóa user", "success");
            return RedirectToAction("trash");
        }

    }
}
