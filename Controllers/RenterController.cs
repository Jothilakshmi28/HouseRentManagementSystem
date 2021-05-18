using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementSystem.Models;
using System.Collections;
using System.Security.Principal;
using System.Data.Entity.Migrations;
using System.Net.Mail;

namespace HouseRentManagementSystem.Controllers
{
    public class RenterController : Controller
    {
        private Database1Entities1 db = new Database1Entities1();
        
       
        // GET: RegisterUsers
        public ActionResult Index()
        {
            
            string Email = User.Identity.Name;
            var obj = db.Renters.FirstOrDefault(e => e.Email.Equals(Email));
            if (obj.ProfileImage != null)
            {
                TempData["userimage"] = Convert.ToBase64String(obj.ProfileImage);
            }
            else
            {
                TempData["userimage"] = null;
            }
            List<Renter> register = new List<Renter>();

            register.Add(obj);

            int SellerID = Convert.ToInt32(Session["SellerID"]);

            return View(register.AsEnumerable());
            
        }
        public ActionResult SoldProperty(int? id)
        {
            //int id = Convert.ToInt32(Session["SellerID"]);
            var props = db.Props.OrderByDescending(e => e.Id).Where(e => e.SellerID == id && e.Availability != "Available");

            return View(props.ToList());
        }
      
        // GET: RegisterUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter registerUser = db.Renters.Find(id);
            if (registerUser == null)
            {
                return HttpNotFound();
            }
            return View(registerUser);
        }
// GET: RegisterUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter registerUser = db.Renters.Find(id);
            if (registerUser == null)
            {
                return HttpNotFound();
            }
            return View(registerUser);
        }
        

        // POST: RegisterUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Password,PhoneNumber,AadharNumber,ProfileImage")] Renter registerUser,HttpPostedFileBase image)
        {
           
           
                if(image!=null)
            {
                registerUser.ProfileImage = new byte[image.ContentLength];
                image.InputStream.Read(registerUser.ProfileImage, 0, image.ContentLength);

            }
            else
            {
                int Id = Convert.ToInt32(Session["SellerID"]);

                var img = db.Renters.Where(m => m.Id == Id).Select(m => m.ProfileImage).FirstOrDefault();
                if (img != null)
                {
                    registerUser.ProfileImage = img;
                }
            }

            if (ModelState.IsValid)
            {
                
               db.Entry(registerUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(registerUser);
        }
        protected override void Dispose(bool disposing)

        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Request(int id)
        {
            Session["propertyid"] = id;
            var customerid = db.Request_Details.Where(m => m.Property_id == id).Select(m=>m.Customer_id);
            var customers = db.Tenants.Where(m => customerid.Contains(m.Id)).ToList();
            return View(customers);
        }
        public ActionResult Confirmation(int cid)
        {
            Session["customerid"] = cid;
            return View();

        }
        [HttpPost]
        public ActionResult Confirmation(Sold_Property sp)
        {
            int deposit = sp.deposit_amount;
            int rent = sp.rent;
            sp.customer_id = (int)Session["customerid"];
            sp.property_id = (int)Session["propertyid"];
            sp.seller_id = (int)Session["SellerID"];
            sp.date_of_sale = DateTime.Now.ToString();
            db.Sold_Property.Add(sp);
           Prop prop= db.Props.Where(e => e.Id == sp.property_id).FirstOrDefault();
            prop.Availability="Not available";
            db.SaveChanges();
            
            int pid = (int)Session["propertyid"];
            int cid = (int)Session["customerid"];
            string cemail = db.Tenants.Where(m => m.Id == cid).Select(m => m.Email).FirstOrDefault();
            var customer = db.Request_Details.Where(m => m.Customer_id == cid && m.Property_id == pid).FirstOrDefault();
            string pname = db.Props.Where(m => m.Id == pid).Select(m => m.PropertyName).FirstOrDefault();
            string paddress = db.Props.Where(m => m.Id == pid).Select(m => m.Address_Line1).FirstOrDefault();
            ConfirmationMail(cemail, pname, paddress,deposit,rent);
            db.Request_Details.Remove(customer);
            db.SaveChanges();
            List<Request_Details> result = db.Request_Details.Where(m => m.Property_id == pid && m.Customer_id != cid).ToList<Request_Details>();
            Prop bd = db.Props.Where(m => m.Id == pid).FirstOrDefault();
            bd.Availability = "Not Available";
            db.SaveChanges();
            if (result != null)
            {
                foreach (Request_Details x in result)
                {
                    string email = db.Tenants.Where(m => m.Id == x.Customer_id).Select(m => m.Email).FirstOrDefault();
                    string propertyname = db.Props.Where(m => m.Id == pid).Select(m => m.PropertyName).FirstOrDefault();
                    string location = db.Props.Where(m => m.Id == pid).Select(m => m.city).FirstOrDefault();
                    RejectionMail(email, propertyname, location);
                    db.Request_Details.Remove(x);
                    db.SaveChanges();
                }
            }

            Session.Remove("propertyid");
            Session.Remove("customerid");
            return RedirectToAction("Index");
        }






        public ActionResult Rejection(int cid)
        {
            
            int pid = (int)Session["propertyid"];
            string cemail = db.Tenants.Where(m => m.Id == cid).Select(m => m.Email).FirstOrDefault();
            var customer = db.Request_Details.Where(m => m.Customer_id == cid && m.Property_id == pid).FirstOrDefault();
            string pname = db.Props.Where(m => m.Id == pid).Select(m => m.PropertyName).FirstOrDefault();
            string location = db.Props.Where(m => m.Id == pid).Select(m => m.city).FirstOrDefault();
            RejectionMail(cemail, pname, location);
            db.Request_Details.Remove(customer);
            db.SaveChanges();
            
            Session.Remove("propertyid");
            Session.Remove("customerid");
            return RedirectToAction("Index");
        }












        public void ConfirmationMail(string emailid, string propertyname, string location,int deposit,int rent)
        {
            MailMessage mailMessage = new MailMessage("jothilakshmijb2000@gmail.com", emailid);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "House Rent - getMyhouse";
            string link = "https://localhost:44313";
            mailMessage.Body = "<html><body><h3> The House that you have requested for " + propertyname + " located at " + location +
                "</h3><p>This property is rented to You and please make the advance payment of &#8377; "+deposit+ " and your house rent will be  &#8377;"+rent+" </p><p style=color:blue;>For further details please contact the Renter.</p><a href=" + link + ">Click Here</a><h2 style=text-align:center>HAPPY HOMING!</h2><img style=float:right src=~/Icons/logo.png></body></html>";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "jothilakshmijb2000@gmail.com",
                Password = "Jothilakshmi2815"
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
        public void RejectionMail(string emailid, string propertyname, string location)
        {
            MailMessage mailMessage = new MailMessage("jothilakshmijb2000@gmail.com", emailid);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "House Rent - getMyhouse";
            string link = "https://localhost:44313";
            mailMessage.Body = "<html><body><h3> The House that you have requested for " + propertyname + " located at " + location +
                "</h3><p>This property is rented to the other customer </p><p style=color:blue;>Do check new houses that suits you.</p><a href=" + link + ">Click Here</a><h2 style=text-align:center>HAPPY HOMING!</h2></body></html>";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "jothilakshmijb2000@gmail.com",
                Password = "Jothilakshmi2815"
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}
