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
    public class BHK : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: Default
        public ActionResult BHK_type()
        {

        List<BHK_type> BHK_Types = db.BHK_type.ToList();
            ViewBag.bhk_types = new SelectList(BHK_Types, "bhk_id", "bhk_name");

            return View();
        }
    }
}