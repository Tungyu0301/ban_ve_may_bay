using BanVeMayBay.Common;
using BanVeMayBay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class SiteController : Controller
    {
        BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]        
        public ActionResult flightSearch(FormCollection fc, int? page)
        {
            
            string typeTicket = fc["typeticket"];
            if (page == null) 
            { 
                page = 1; 
            }
            int pageSize = 4;
          
            int songuoi1 = int.Parse(fc["songuoi1"]);
            int songuoi2 = int.Parse(fc["songuoi2"]);
            int songuoi3 = int.Parse(fc["songuoi3"]);
            int tong = songuoi1 + songuoi2 + songuoi3;
            int songuoi = tong;
            ViewBag.songuoi = songuoi;
            string noiBay = fc["departure_address"];
            string noiVe = fc["arrival_address"];
            string ngaybay = fc["departure_date"];
           
            ViewBag.url = "chuyen-bay";
            //convert sang mm/dd/yy cho may hieu 
            DateTime ngaybay1 = DateTime.ParseExact(ngaybay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //sang mm/dd/yy
            string ngaybay2 = ngaybay1.ToString("MM-dd-yyyy");
            DateTime ngaybay3 = DateTime.Parse(ngaybay2);
            ViewBag.noiBay = noiBay;
            ViewBag.noiVe = noiVe;
            ViewBag.ngaybay = ngaybay;
           
            // neu la ve 2 chieu
            if (typeTicket.Equals("enable"))
            {
                string ngayve = fc["arrival_date"];
                DateTime ngayden1 = DateTime.ParseExact(ngayve, "d/M/yyyy", CultureInfo.InvariantCulture);
                string ngayden2 = ngayden1.ToString("MM-dd-yyyy");
                DateTime ngayden3 = DateTime.Parse(ngayden2);
                ViewBag.ngayden = ngayve;
                ViewBag.date = ngayden3;

                if (ngaybay1 > ngayden1)
                {
                    Message.set_flash("Ngày về phải lớn hơn hoặc bằng ngày đi!", "danger");
                    return Redirect("~/Home/Index");
                }
                var list = db.tickets.Where(m => m.city.cityName.Contains(noiBay) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m => m.status == 1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchReturn", list.ToPagedList(pageNumber, pageSize));
                    
                
            }
            else
            {

                //ve 1 chieu
                var list = db.tickets.Where(m => m.city.cityName.Contains(noiBay) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m=>m.status==1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchOnway", list.ToPagedList(pageNumber, pageSize));
            
            }

        }
        
        public ActionResult return_ticket(DateTime date,string noibay, string noiden)
        {
           
            var list = db.tickets.Where(m => m.city.cityName.Contains(noiden) && m.city1.cityName.Contains(noibay)).
               Where(m => m.departure_date == date).Where(m => m.status == 1).ToList();
            return View("_returnTicket", list);
        }
        public ActionResult AllChuyenBay(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            //var singleC = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).First();
            ViewBag.url = "all-chuyen-bay";
            int pageNumber = (page ?? 1);
            //không biết sử dụng
            //ViewBag.breadcrumb = "Tất cả chuyến bay";//không biết sử dụng
            var list_flight = db.tickets.Where(m => m.status == 1).ToList();
            return View("allflight", list_flight.ToPagedList(pageNumber, pageSize));
        }        
            public ActionResult flightDetail(int id)
        {
            var single = db.tickets.Where(m => m.status == 1 && m.id == id).First();
            return View("flightDetail", single);
        }
    }
}