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
    public class StatesController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: States
        public ActionResult Index()
        {
            List<State> states = db.States.ToList();
            ViewBag.states = new SelectList(states, "stateid", "state_name");

            return View();
        }

        public ActionResult BHK_type()
        {

            List<BHK_type> BHK_Types = db.BHK_type.ToList();
            ViewBag.bhk_types = new SelectList(BHK_Types, "bhk_id", "bhk_name");

            return View();
        }
        public JsonResult GetCityList(int stateid)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<city> cities = db.cities.Where(e=>e.stateid==stateid).ToList();
            return Json(cities,JsonRequestBehavior.AllowGet);

        }

      
    }
}