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
    public class TenantController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();

        // GET: 

        public ActionResult Index()
        {

            string Email = User.Identity.Name;
            var obj = db.Tenants.FirstOrDefault(e => e.Email.Equals(Email));

            if (obj.ProfileImage != null)
            {
                TempData["userimage"] = Convert.ToBase64String(obj.ProfileImage);
            }
            else
            {
                TempData["userimage"] = null;
            }
            List<Tenant> register = new List<Tenant>();

            register.Add(obj);
            int UserID = Convert.ToInt32(Session["UserID"]);

            return View(register.AsEnumerable());

         
        }

        // GET: Tenant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenant/Create
     

        // POST: Tenant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNumber,AadharNumber")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("IndexTenant");
            }

            return View(tenant);
        }
        public ActionResult RequestSent()
        {
            return View();
        }

        public ActionResult RequestCancelled()
        {
            return View();
        }

        // GET: Tenant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNumber,AadharNumber,ProfileImage")] Tenant tenant, HttpPostedFileBase image)
        {
            if (image != null)
            {
                tenant.ProfileImage = new byte[image.ContentLength];
                image.InputStream.Read(tenant.ProfileImage, 0, image.ContentLength);

            }
            else
            {
                int Id = Convert.ToInt32(Session["UserID"]);

                var img = db.Tenants.Where(m => m.Id == Id).Select(m => m.ProfileImage).FirstOrDefault();
                if (img != null)
                {
                    tenant.ProfileImage = img;
                }
            }


            if (ModelState.IsValid)
            {

                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexTenant","Account");

            }
            return View(tenant);
        }

   



               

        // GET: Tenant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
            db.SaveChanges();
            return RedirectToAction("IndexTenant");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult TopSeller()
        {
            var sellers = from s in db.Sold_Property group s by s.seller_id;
            var topseller = sellers.OrderByDescending(m => m.Count()).Take(3);
            var topsellerid = topseller.Select(m => m.Key);
            var seller = db.Renters.Where(m => topsellerid.Contains(m.Id)).ToList();
            List<int> count = new List<int>();
            foreach (var item in topsellerid)
            {
                count.Add(db.Sold_Property.Count(m => m.seller_id == item));
            }
            ViewBag.count = count;
            return View(seller);
        }
    }
}
