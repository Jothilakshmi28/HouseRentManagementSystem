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
    public class AdminPropertyController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: AdminProperty
        public ActionResult Index()
        {
            var props = db.Props.Include(p => p.Renter);
            return View(props.ToList());
        }

        // GET: AdminProperty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prop prop = db.Props.Find(id);
            if (prop == null)
            {
                return HttpNotFound();
            }
            return View(prop);
        }

        // GET: AdminProperty/Create
        public ActionResult Create()
        {
            ViewBag.SellerID = new SelectList(db.Renters, "Id", "FirstName");
            return View();
        }

        // POST: AdminProperty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BHK_type,Id,House_type,Address_Line1,Address_Line2,city,state,Rent,SellerID,Availability,SquareFeet,PropertyName")] Prop prop)
        {
            if (ModelState.IsValid)
            {
                db.Props.Add(prop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SellerID = new SelectList(db.Renters, "Id", "FirstName", prop.SellerID);
            return View(prop);
        }

        // GET: AdminProperty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prop prop = db.Props.Find(id);
            if (prop == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerID = new SelectList(db.Renters, "Id", "FirstName", prop.SellerID);
            return View(prop);
        }

        // POST: AdminProperty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BHK_type,Id,House_type,Address_Line1,Address_Line2,city,state,Rent,SellerID,Availability,SquareFeet,PropertyName")] Prop prop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SellerID = new SelectList(db.Renters, "Id", "FirstName", prop.SellerID);
            return View(prop);
        }

        // GET: AdminProperty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prop prop = db.Props.Find(id);
            if (prop == null)
            {
                return HttpNotFound();
            }
            return View(prop);
        }

        // POST: AdminProperty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Image> image = db.Images.Where(m => m.PropertyId == id).ToList();
            foreach (var i in image)
            {
                db.Images.Remove(i);
            }
            Prop prop = db.Props.Find(id);
            db.Props.Remove(prop);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
