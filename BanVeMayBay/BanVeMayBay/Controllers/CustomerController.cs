using BanVeMayBay.Common;
using BanVeMayBay.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace BanVeMayBay.Controllers
{
    public class CustomerController : Controller
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        // GET: Customer
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(user loginuser, FormCollection fc)
        {
            String Username = fc["username"];
            String Pass = (fc["password"]);
            var y = db.users.Where(m => m.access == 1 && m.status == 1 && (m.username == Username)).FirstOrDefault();
            var x = db.users.Where(m => m.access == 0 && m.status == 1 && (m.username == Username)).FirstOrDefault();


            if (y != null)
            {
                Session.Add(CommonConstants.CUSTOMER_SESSION, y);
                Session["userName11"] = y.fullname;
                Session["id"] = y.ID;
                if (!Response.IsRequestBeingRedirected)
                    Message.set_flash("Đăng nhập thành công", "success");
                return Redirect("~/tai-khoan");
            }
            else if (x != null)
            {
                Session["Admin"] = loginuser.username;
                role role = db.roles.Where(m => m.parentId == x.access).First();
                var userSession = new Userlogin();
                userSession.UserName = x.username;
                userSession.UserID = x.ID;
                userSession.GroupID = role.GropID;
                userSession.AccessName = role.accessName;
                Session.Add(CommonConstants.USER_SESSION, userSession);
                var i = Session["SESSION_CREDENTIALS"];
                Session["Admin_id"] = x.ID;
                Session["Admin_user"] = x.username;
                Session["Admin_fullname"] = x.fullname;
                Response.Redirect("~/Admin");
            }
            else
            {
                ViewBag.m = "Sai tài khoản hoặc mật khẩu";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
        public void logout()
        {
            Session["userName11"] = "";
            Session[Common.CommonConstants.CUSTOMER_SESSION] = "";
            Response.Redirect("~/dang-nhap");
            Message.set_flash("Đăng xuất thành công", "success");
        }
        public ActionResult register()
        {
            return View("register");
        }
        [HttpPost]
        public ActionResult register(user muser, FormCollection fc)
        {
            string uname = fc["uname"];
            string fname = fc["fname"];
            string Pass = fc["psw"];
            string Pass2 =fc["repsw"];
            if (Pass2 != Pass)
            {
                ViewBag.error = " Sai password ";
                return View("register");
            }
            string email = fc["email"];
            string address = fc["address"];
            string phone = fc["phone"];
            if (ModelState.IsValid)
            {
                var Luser = db.users.Where(m => m.status == 1 && m.username == uname && m.access == 1);
                if (Luser.Count() > 0)
                {
                    ViewBag.error = "Username đã tồn tại";
                    return View("register");
                }
                else
                {
                    muser.img = "defalt.png";
                    muser.password = Pass;
                    muser.username = uname;
                    muser.fullname = fname;
                    muser.email = email;
                    muser.address = address;
                    muser.phone = phone;
                    muser.gender = "nam";
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = 1;
                    muser.updated_by = 1;
                    muser.access = 1;
                    muser.status = 1;
                    db.users.Add(muser);
                    db.SaveChanges();
                    Message.set_flash("Đăng ký thành công ", "success");
                    return Redirect("~/dang-nhap");
                }

            }
            Message.set_flash("Đăng ký thất bại", "danger");
            return View("register");
        }

        public ActionResult Myaccount()
        {
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            return View("Myaccount", sessionUser);
        }
        [HttpPost]
        public ActionResult Myaccount(user user, FormCollection fc)
        {
            var pswO = fc["pswO"];
            var pswN = fc["pswN"];
            var pswR = fc["pswR"];
            if (pswO != null)
            {
                if (pswO != user.password)
                {
                    ViewBag.success = "Pass cũ không chính xác.";
                    return View("Myaccount", user);
                }
                if (pswN == null || pswR == null || pswN.Length < 6 || pswR.Length < 6)
                {
                    ViewBag.success = "Pass mới không hợp lệ.";
                    return View("Myaccount", user);
                }
                if (pswN != pswR)
                {
                    ViewBag.success = "Pass không trùng.";
                    return View("Myaccount", user);
                }
                else
                {
                    user.password = pswN;
                }
            }

            Session[Common.CommonConstants.CUSTOMER_SESSION] = "";
            Session.Add(CommonConstants.CUSTOMER_SESSION, user);
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;
            user.created_by = 1;
            user.updated_by = 1;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.success = "Update thành công.";
            return View("Myaccount", user);
        }
        public ActionResult ListOderCus()
        {

            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            var listOrder = db.orders.Where(m => m.CusId == sessionUser.ID).OrderByDescending(m => m.ID).ToList();
            return View("ListOderCus", listOrder);
        }
        public ActionResult orderDetailCus(int id)
        {
            var sigleOrder = db.orders.Find(id);
            return View("orderDetailCus", sigleOrder);
        }
        public ActionResult canelOrder(int OrderId)
        {


            order morder = db.orders.Find(OrderId);
            var orderDetail = db.ordersdetails.Where(m => m.orderid == morder.ID).ToList();
            foreach (var item in orderDetail)
            {
                var id = int.Parse(item.ticketId.ToString());
                ticket ticket = db.tickets.Find(id);
                DateTime ngaymuon = Convert.ToDateTime(
                    morder.created_ate);
                DateTime ngaytra = Convert.ToDateTime(ticket.departure_date);
                TimeSpan Time = ngaytra - ngaymuon;
                int TongSoNgay = Time.Days;
                if (TongSoNgay >= 14)
                {
                    ticket.Sold = ticket.Sold - item.quantity;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    if (item == null)
                    {
                        Message.set_flash("Hủy thất bại", "danger");
                        return Redirect("~/tai-khoan");
                    }
                    db.ordersdetails.Remove(item);
                    db.SaveChanges();
                }
                else
                {
                    Message.set_flash("Vé không thể xóa nếu cận 14 ngày bay", "dangger");
                    return Redirect("~/tai-khoan");
                }


            }

            db.orders.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Đã hủy", "success");
            return Redirect("~/tai-khoan");
        }
    }
}