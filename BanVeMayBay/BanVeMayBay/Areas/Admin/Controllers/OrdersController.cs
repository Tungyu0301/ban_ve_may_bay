using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVeMayBay.Common;
using BanVeMayBay.Models;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            ViewBag.list = db.ordersdetails.ToList();
            return View(db.orders.Where(m=>m.status!=0).ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
       

   
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

        //status
        public ActionResult Status(int id)
        {
            order morder = db.orders.Find(id);
            morder.status = (morder.status == 1) ? 2 : 1;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.orders.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            order morder = db.orders.Find(id);
            morder.status = 0;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Retrash(int id)
        {
            order morder = db.orders.Find(id);
            morder.status = 2;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Phục hồi thành côcng", "success");
            return RedirectToAction("trash");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult deleteTrash(int id)
        {
            order morder = db.orders.Find(id);
            var orderDetail = db.ordersdetails.Where(m => m.orderid == morder.ID).ToList();
            foreach (var item in orderDetail)
            {
                var idd = int.Parse(item.ticketId.ToString());
                ticket ticket = db.tickets.Find(idd);
                ticket.Sold = ticket.Sold - item.quantity;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                db.ordersdetails.Remove(item);
                db.SaveChanges();
            }
            db.orders.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Đã xóa Order", "success");
            return RedirectToAction("trash");
        }
    }
}
