using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementSystem.Models;

namespace HouseRentManagementSystem.Controllers
{
     public class AdminController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();


        // GET: Admin
        public ActionResult AdminDashboard()
        {
           
                ViewBag.PropertyCount = db.Props.Where(e=>e.Id!=0).Count();
            ViewBag.RenterCount = db.Renters.Where(e => e.Id != 0).Count();
            ViewBag.TenantCount = db.Tenants.Where(e => e.Id != 0).Count();
            ViewBag.SoldCount = db.Props.Where(e => e.Id != 0 && e.Availability != "Available").Count();
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(string id,string password)
        {
            if(id=="getmyhouseadmin_1@gmail.com" && password=="getMyhouse")
            {
                return RedirectToAction("AdminDashboard");
            }
           else if (id == "getmyhouseadmin_1@gmail.com" && password != "getMyhouse")
            {
                ViewBag.Error = "INCORRECT PASSWORD";
               
            }
           else if (id != "getmyhouseadmin_1@gmail.com" && password != "getMyhouse")
            {
                ViewBag.Error = "INCORRECT LOGIN CREDENTIALS";
                
            }
            return View();
        }
       
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("AdminLogin", "Admin");
        }

    }
}