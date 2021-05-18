using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementSystem.Models;

namespace HouseRentManagementSystem.Views.AdminRenter
{
    public class AdminRenterController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: AdminRenter
        public ActionResult Index()
        {
            return View(db.Renters.ToList());
        }

        // GET: AdminRenter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter Renter = db.Renters.Find(id);
            if (Renter == null)
            {
                return HttpNotFound();
            }
            return View(Renter);
        }

        // GET: AdminRenter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminRenter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNumber,AadharNumber")] Renter Renter)
        {
            if (ModelState.IsValid)
            {
                db.Renters.Add(Renter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Renter);
        }

        // GET: AdminRenter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter Renter = db.Renters.Find(id);
            if (Renter == null)
            {
                return HttpNotFound();
            }
            return View(Renter);
        }

        // POST: AdminRenter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNumber,AadharNumber")] Renter Renter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Renter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Renter);
        }

        // GET: AdminRenter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter Renter = db.Renters.Find(id);
            if (Renter == null)
            {
                return HttpNotFound();
            }
            return View(Renter);
        }

        // POST: AdminRenter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
              List<Prop> propid = db.Props.Where(e => e.SellerID == id).ToList();
                foreach (var i in propid)
                {

                    List<Image> image = db.Images.Where(m => m.PropertyId == i.Id).ToList();
                    foreach (var j in image)
                    {
                        db.Images.Remove(j);
                        db.Props.Remove(i);
                    }

                }
                Renter renter = db.Renters.Find(id);
                db.Renters.Remove(renter);
                
            
           
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
