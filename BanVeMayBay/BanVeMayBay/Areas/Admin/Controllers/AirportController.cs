using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVeMayBay.Common;
using BanVeMayBay.Models;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class AirportController : BaseController
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        // GET: Admin/Tickets
        public ActionResult Index()
        {
            var cities = db.cities.Where(m => m.status == 1).ToList();
            ViewBag.cities = cities;
            return View();
        }


        // GET: Admin/Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Admin/Tickets/Create
        public ActionResult Create()
        {
            var countries = db.countries.Where(m =>m.status == 1).ToList();
            ViewBag.countries = countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(country countries)
        {           
            //if(db.countries.Where(m=>m.countryName.Contains(countries.countryName)) == null)
            //    {
                    db.countries.Add(countries);
                    Message.set_flash("Thêm nước thành công", "success");
                    db.SaveChanges();
                    return RedirectToAction("Index");
                //}
                //else
                //{
                //    Message.set_flash("Country already exist", "danger");
                //    return View("Create");
                //}

            
        }

        //GET: Admin/Tickets/Create
        public ActionResult CreateCities()
        {
            var countries = db.countries.Where(m=>m.status == 1).ToList();
            ViewBag.countries = countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCities(city cities)
        {

            if (ModelState.IsValid)
            {
                db.cities.Add(cities);
                Message.set_flash("Thêm thành phố thành công", "success");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Message.set_flash("thêm thất bại", "danger");
            return View("Create");
        }


        public ActionResult Status(int id)
        {
            city cities = db.cities.Find(id);
            cities.status = (cities.status == 1) ? 2 : 1;
            db.Entry(cities).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Đổi trạng thái thàng công", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.cities.Where(m => m.status == 2).ToList();
            return View("Trash", list);
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Deltrash(int id)
        {
            city morder = db.cities.Find(id);
            morder.status = 2;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("xóa thành công", "success");
            return RedirectToAction("Index");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult Retrash(int id)
        {
            city morder = db.cities.Find(id);
            morder.status = 1;
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Hồi phục thành công", "success");
            return RedirectToAction("trash");
        }
        [CustomAuthorizeAttribute(RoleID = "ADMIN")]
        public ActionResult deleteTrash(int id)
        {
            city morder = db.cities.Find(id);
            db.cities.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Đã xóa user", "success");
            return RedirectToAction("trash");
        }


    }
}
