using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementSystem.Models;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
using PagedList;
 

namespace HouseRentManagementSystem.Controllers
{
    public class PropsController : Controller
    {
        
        private Database1Entities1 db = new Database1Entities1();

       
        // GET: Props
        public ActionResult Index()
        {
            
            int SellerID = Convert.ToInt32(Session["SellerID"]);
            // return View(db.Props.ToList().Where(e=>e.SellerID== SellerID));         
            int count = db.Props.Count();
           
                        return View(db.Props.Where(e => e.SellerID == SellerID).ToList());

        }
        public JsonResult Autocomplete(string Prefix)
        {
            var property_city = (from p in db.Props
                                 where p.city.StartsWith(Prefix)
                                 select new
                                 {
                                     label = p.city,
                                     val = p.city
                                 }).ToList();
            return Json(property_city);

        }
        public ActionResult Search(string search)
        {
            Session["search"] = search;
            if (Session["min"] != null)
            {
                int min = (int)Session["min"];
                int max = (int)Session["max"];
                List<Prop> property = db.Props.Where(m => m.city == search&& m.Rent>=min &&m.Rent<= max && m.Availability == "Available").ToList<Prop>();
                return View(property);
            }
            else
            {
                List<Prop> property = db.Props.Where(m => m.city == search && m.Availability == "Available").ToList<Prop>();
                return View(property);
            }


        }
        public ActionResult PriceRange(int min,int max)
        {
            Session["min"] = min;
            Session["max"] = max;
            if (min != 25000 && max != 25000)
            {
                if (Session["search"] != null)
                {
                    string city = (string)Session["search"];
                    var result = db.Props.Where(m => m.Rent >= min && m.Rent <= max && m.city == city && m.Availability == "Available").ToList();
                    return View(result);
                }
                else
                {
                    var result = db.Props.Where(m => m.Rent >= min && m.Rent <= max && m.Availability == "Available").ToList();
                    return View(result);
                }
            }
            else
            {
                var result = db.Props.Where(m => m.Rent >= min && m.Availability == "Available").ToList();
                return View(result);
            }
        }
       
        public ActionResult Sort(int price)
        {
            Session["price"] = price;
         
                if (Session["search"] == null && Session["min"] == null && Session["max"] == null)
                {
                    if (price == 1)
                    {
                        var result = db.Props.OrderBy(m => m.Rent).Where(m => m.Availability == "Available").ToList();
                        return View(result);
                    }
                    if (price == 2)
                    {
                        var result = db.Props.OrderByDescending(m => m.Rent).Where(m => m.Availability == "Available").ToList();
                        return View(result);
                    }
                }
                else if (Session["search"] != null && Session["min"] == null && Session["max"] == null)
                {
                    if (price == 1)
                    {
                        string city = (string)Session["search"];
                        var result = db.Props.OrderBy(m => m.Rent).Where(m => m.Availability == "Available" && m.city == city).ToList();
                        return View(result);
                    }
                    if (price == 2)
                    {
                        string city = (string)Session["search"];
                        var result = db.Props.OrderByDescending(m => m.Rent).Where(m => m.Availability == "Available" && m.city == city).ToList();
                        return View(result);
                    }
                }
          else if(Session["search"] == null && Session["min"] != null && Session["max"] != null)
            {
               
                if(price==1)
                {
                    int min = (int)Session["min"];
                    int max = (int)Session["max"];
                    var result = db.Props.OrderBy(m => m.Rent).Where(m => m.Availability == "Available" && m.Rent >= min&& m.Rent<=max).ToList();
                    return View(result);

                }
                if (price == 2)
                {
                    int min = (int)Session["min"];
                    int max = (int)Session["max"];
                    var result = db.Props.OrderByDescending(m => m.Rent).Where(m => m.Availability == "Available" && m.Rent >= min && m.Rent <= max).ToList();
                    return View(result);
                }
            }

            else
            {
                int min = (int)Session["min"];
                int max = (int)Session["min"];
                string city = (string)Session["search"];
                if(price==1)
                {
                    var result = db.Props.OrderBy(m => m.Rent).Where(m => m.Availability == "Available" && m.Rent >= min && m.Rent <= max && m.city==city).ToList();
                    return View(result);

                }
                if (price == 2)
                {
                    var result = db.Props.OrderByDescending(m => m.Rent).Where(m => m.Availability == "Available" && m.Rent >= min && m.Rent <= max && m.city == city).ToList();
                    return View(result);

                }

            }

            return View();
        }
        public ActionResult UserIndex(int? page)
        { 
            var pageNumber = page ?? 1;
                var pageSize = 6;
          


            int uid = Convert.ToInt32(Session["userID"]);
            var result = db.Request_Details.Where(m => m.Customer_id == uid).Select(m => m.Property_id);
      
        var prop = db.Props.Where(m => !result.Contains(m.Id) && m.Availability=="Available");
            var obj = prop.OrderBy(m => m.Id).ToPagedList(pageNumber, pageSize);


            var sellers = from s in db.Sold_Property group s by s.seller_id;
            var topseller = sellers.OrderByDescending(m => m.Count()).Take(3);

            var topsellerid = topseller.Select(m => m.Key);
            var seller = db.Renters.Where(m => topsellerid.Contains(m.Id));
            List<int> count = new List<int>();
            List<Renter> list = new List<Renter>();
            foreach (var item in topsellerid)
            {
                count.Add(db.Sold_Property.Count(m => m.seller_id == item));
            }



            ViewBag.count = count;
            ViewBag.topseller = seller;
            return View(obj);
        }
        [HttpPost]
        public ActionResult UserIndex()
        {
           
            
            return View();
        }
    public ActionResult CustomerRequest()
        {
            int SellerID = Convert.ToInt32(Session["SellerID"]);
            var prop1 = db.Props.Where(e => e.SellerID == SellerID);
            
                if (prop1 != null)
            {
              var  prop = db.Tenants.Where(e => e.Id == SellerID);
                return View(prop.ToList());
            }
            return View();
        }
        public ActionResult UserIndexs(int id,int sid)
        {            
            int uid = Convert.ToInt32(Session["userID"]);

            Request_Details rd = new Request_Details
            {
                Seller_Id = sid,
                Property_id = id,
                Customer_id = uid
            };
            db.Request_Details.Add(rd);
            db.SaveChanges();
            return RedirectToAction("RequestedProperty");
        }
        public ActionResult RequestedProperty()
        {
            int uid = Convert.ToInt32(Session["userID"]);
        var result = db.Request_Details.Where(m => m.Customer_id == uid).Select(m => m.Property_id);
        var prop = db.Props.Where(m => result.Contains(m.Id)).ToList();
            return View(prop);
        }

        public ActionResult Images(int id)
        {
            var prop = db.Props.Where(e => e.Id == id);
            return View(prop.ToList());
        }

        public ActionResult Image(int id)
        {
            var prop = db.Props.Where(e => e.Id == id);
            return View(prop.ToList());
        }
        public ActionResult EditImages(int id)
        {
            Session["prop_id"] = id;
            var prop = db.Props.Where(e => e.Id == id).ToList();
            return View(prop);
        }
        [HttpPost]
        public ActionResult EditImages(HttpPostedFileBase[] image)
        {
            if (image != null)
            {
                foreach (var images in image)
                {

                    BinaryReader br = new BinaryReader(images.InputStream);
                    Image img = new Image
                    {
                        PropertyId = (int)Session["prop_id"],
                        PropertyImage = br.ReadBytes((int)images.ContentLength)
                    };
                    // Prop.Images = new List<Image> { img };
                    db.Images.Add(img);
                }
                db.SaveChanges();
            }

            return RedirectToAction("EditImages", new { id = (int)Session["prop_id"] });

        }
        public ActionResult UserCancels(int id,int sid)
        {
            int uid = Convert.ToInt32(Session["userID"]);
            Request_Details rd = db.Request_Details.Where(
                m => m.Customer_id == uid && m.Property_id == id && m.Seller_Id == sid).FirstOrDefault();

            db.Request_Details.Remove(rd);

            db.SaveChanges();
           
            return RedirectToAction("RequestedProperty");
        }
// GET: Props/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prop prop = db.Props.Find(id);
            if(prop!=null)
            {
                Session["PropertyID"] = Convert.ToInt32(prop.Id);
            }
            if (prop == null)
            {
                return HttpNotFound();
            }
            return View(prop);
        }

        public ActionResult Detail(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prop prop = db.Props.Find(id);
            if (prop != null)
            {
                Session["PropertyID"] = Convert.ToInt32(prop.Id);
            }
            if (prop == null)
            {
                return HttpNotFound();
            }
            return View(prop);
        }

        // GET: Props/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Props/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prop prop,HttpPostedFileBase[] images)
        {
            var SellerID = Session["SellerID"];
            try
            {
                if (ModelState.IsValid)
                {
                    prop.SellerID = Convert.ToInt32(SellerID);
                    prop.Availability = "Available";
                    db.Props.Add(prop);
                    db.SaveChanges();
                    int id = db.Props.Max(e => e.Id);
                    if (images != null)
                    {
                        foreach (var image in images)
                        {

                            BinaryReader br = new BinaryReader(image.InputStream);
                            Image img = new Image
                            {
                                PropertyId = id,
                                PropertyImage = br.ReadBytes((int)image.ContentLength)
                            };
                            // Prop.Images = new List<Image> { img };
                            db.Images.Add(img);
                        }
                            db.SaveChanges();

                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
                return RedirectToAction("Index","Account");
            }
           
          
        

        // GET: Props/Edit/5
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
            
            return View(prop);
        }

        // POST: Props/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Prop prop,string avail) { 
            if (ModelState.IsValid)
            {
                prop.Availability = avail;
                db.Entry(prop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prop);
        }

        // GET: Props/Delete/5
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

        // POST: Props/Delete/5
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
        public ActionResult RemoveImages(int id)
        {
            int propid = (int)Session["prop_id"];
            var img = db.Images.Where(m => m.Id == id).FirstOrDefault();
            db.Images.Remove(img);
            db.SaveChanges();
            return RedirectToAction("EditImages", new { id = (int)Session["prop_id"] });
        }
        
          
    }
}
