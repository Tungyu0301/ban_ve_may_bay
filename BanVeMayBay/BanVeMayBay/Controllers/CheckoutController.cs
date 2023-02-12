using BanVeMayBay.Common;
using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class CheckoutController : Controller
    {
        BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        public ActionResult Invalid()
        {
            return View("Invalid");
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            String Username = fc["username"];
            String Pass = fc["password"];
            var user_account = db.users.Where(m => m.access == 1 && m.status == 1 && (m.username == Username));
            var pass = user_account.FirstOrDefault()?.password;
            if (user_account.Count() == 0)
            {
                ViewBag.error = "Sai Username ";
            }
            else
            {
                var pass_account = user_account.Where(m => m.access == 1 && m.status == 1 && m.password == Pass).FirstOrDefault();
                if (pass_account == null)
                {
                    ViewBag.error = "Sai password";
                }
                else
                {
                    var user = user_account.First();
                    Session.Add(CommonConstants.CUSTOMER_SESSION, user);
                    Session["userName11"] = user.fullname;
                    Session["id"] = user.ID;
                    if (!Response.IsRequestBeingRedirected)
                        Message.set_flash("Đăng nhập thành công ", "success");
                    return Redirect("~/Home/Index");
                }
            }
            return View("Invalid");

        }
        // GET: Checkout
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            var iddd = 0;
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                iddd = sessionUser.ID;
                var list = new List<ticket>();

                if (fc["datve"] != null)
                {
                    int id = int.Parse(fc["datve"]);
                    var list1 = db.tickets.Find(id);
                    ViewBag.songuoi = int.Parse(fc["songuoi"]);

                    list.Add(list1);
                    ViewBag.ve1 = id;
                }
                else
                {
                    return Redirect("~/Home/index");
                }

                // neu co ve khu hoi
                if (!string.IsNullOrEmpty(fc["datveKH"]))
                {
                    int id2 = int.Parse(fc["datveKH"]);
                    var list2 = db.tickets.Find(id2);
                    ViewBag.ve2 = id2;
                    list.Add(list2);
                }


                return View("", list.ToList());
            }
            else
            {

                return View("Invalid");
            }
            //dừng..

        }

        [HttpPost]
        public ActionResult checkOut(order order, FormCollection fc)
        {
            var iddd = 0;
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                iddd = sessionUser.ID;
            }
            float total = float.Parse(fc["total"]);
            order.created_ate = DateTime.Now;
            order.status = 1;
            order.total = total;
            order.CusId = iddd;
            db.orders.Add(order);
            db.SaveChanges();
            int lastOrderID = order.ID;
            ordersdetail orderDetail = new ordersdetail();
            int id1 = int.Parse(fc["veOnvay"]);
            orderDetail.ticketId = id1;
            orderDetail.quantity = order.guestTotal;
            orderDetail.orderid = lastOrderID;
            db.ordersdetails.Add(orderDetail);
            // tru so luong nghe
            var ticket = db.tickets.Find(id1);
            ticket.Sold = ticket.Sold + order.guestTotal;
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
            //neu ton tai ve 2 chieu
            if (!string.IsNullOrEmpty(fc["veReturn"]))
            {
                int id2 = int.Parse(fc["veReturn"]);
                ordersdetail orderDetail2 = new ordersdetail();
                orderDetail2.ticketId = id2;
                orderDetail2.orderid = lastOrderID;
                orderDetail2.quantity = order.guestTotal;
                db.ordersdetails.Add(orderDetail2);
                // tru so luong nghe
                var ticket2 = db.tickets.Find(id2);
                ticket2.Sold = ticket2.Sold + order.guestTotal;
                db.Entry(ticket2).State = EntityState.Modified;
                db.SaveChanges();
            }



            return View("checkOutComfin", order);
        }
        // lay thong tin cac ve da book
        public ActionResult _BookingConnfig(int orderId)
        {
            var list = db.ordersdetails.Where(m => m.orderid == orderId).ToList();
            var list1 = new List<ticket>();
            foreach (var item in list)
            {
                ticket ticket = db.tickets.Find(item.ticketId);
                list1.Add(ticket);
            }

            return View("_BookingConnfig", list1.ToList());
        }

    }
}