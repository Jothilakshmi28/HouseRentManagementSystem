using HouseRentManagementSystem.Models;
using HouseRentManagementSystem.ViewModel;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Net.Mail;

using System.Configuration;



using System.Data;

using System.IO;

namespace HouseRentManagementSystem.Controllers
{
    
    public class AccountController : Controller
    {
        // Return Home page.
        public ActionResult HomePage()
        {
            return View();
        }
       public ActionResult Index()
        {
            string Email = User.Identity.Name;
            Database1Entities1 db = new Database1Entities1();
            var obj = db.Renters.FirstOrDefault(e => e.Email.Equals(Email));
            Session["Name"] = Convert.ToString(obj.FirstName + " " + obj.LastName);
            ViewBag.image = obj.ProfileImage;

Session["SellerID"] = Convert.ToInt32(obj.Id);
            var props = db.Props.OrderByDescending(e => e.Id).Where(e=>e.SellerID==obj.Id && e.Availability=="Available");

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


            return View(props.ToList());
        }
        
        public ActionResult IndexTenant()
        {
               
            string Email = User.Identity.Name;
            Database1Entities1 db = new Database1Entities1();
            var obj = db.Tenants.FirstOrDefault(e => e.Email.Equals(Email));
            ViewBag.image = obj.ProfileImage;

            Session["UserName"] = Convert.ToString(obj.FirstName + " " + obj.LastName);
            Session["UserID"] = Convert.ToInt32(obj.Id);
            var props = db.Props.Where(e=>e.Availability=="Available").OrderByDescending(e=>e.Id).Take(6);


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


            return View(props.ToList());
        }

        
      

        //Return Register view
        public ActionResult RegisterRenter()
        {
            return View();
        }
        public ActionResult RegisterTenant()
        {
            return View();
        }
        public ActionResult LoginTenant()
        {
            return View();
        }
       public ActionResult ForgotPassword()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            Database1Entities1 databaseContext = new Database1Entities1();
            var userWithSameEmail = databaseContext.Renters.Where
                       (m => m.Email == email).FirstOrDefault(); //checking if the emailid already exits for any user
                                                                 //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                                                                              //Save all details in RegitserUser object
            if (userWithSameEmail == null)
            {
               
                 ViewBag.Message = "Email does not exists";
                ViewBag.Error = "Your mail ID is not registered.So please continue with registeration";
            }
            else
            {
                Session["email"] = Convert.ToString(email);
                MailMessage mailMessage = new MailMessage("jothilakshmijb2000@gmail.com", email);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Reset Password - getMyHouse";
                string link = "https://localhost:44313/Account/ResetPassword";

                mailMessage.Body = "<html><body><h1>Hi, Click the below link to reset your password</h1><a href=" + link + "><p>Click Here</p></a></body></html>";


                mailMessage.Body = "<html><body><h1> Hi, Click the below link to reset your password</h1><a href="+link+">Click Here</a></body></html>";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "jothilakshmijb2000@gmail.com",
                    Password = "Jothilakshmi2815"
                };

                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return RedirectToAction("MailExists");
            }
            return View();
        }

        public ActionResult Forgot_Password()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Forgot_Password(string email)
        {
            Database1Entities1 databaseContext = new Database1Entities1();
            var userWithSameEmail = databaseContext.Tenants.Where
                       (m => m.Email == email).FirstOrDefault(); //checking if the emailid already exits for any user
                                                                 //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                                                                 //Save all details in RegitserUser object
            if (userWithSameEmail == null)
            {

                ViewBag.Message = "Email does not exists";
                ViewBag.Error = "Your mail ID is not registered.So please continue with registeration";
            }
            else
            {
                Session["email"] = Convert.ToString(email);
                MailMessage mailMessage = new MailMessage("jothilakshmijb2000@gmail.com", email);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Reset Password - getMyHouse";
                string link = "https://localhost:44313/Account/Reset_Password";

                mailMessage.Body = "<html><body><h1>Hi, Click the below link to reset your password</h1><a href=" + link + "><p>Click Here</p></a></body></html>";


                mailMessage.Body = "<html><body><h1> Hi, Click the below link to reset your password</h1><a href=" + link + ">Click Here</a></body></html>";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "jothilakshmijb2000@gmail.com",
                    Password = "Jothilakshmi2815"
                };

                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return RedirectToAction("MailExists");
            }
            return View();
        }


        public ActionResult MailExists()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {

            return View();
        }
        public ActionResult Reset_Password()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string password,Renter renter)
        {
            String email = Convert.ToString(Session["email"]);
            Database1Entities1 databaseContext = new Database1Entities1();
            var userWithSameEmail = databaseContext.Renters.Where
                       (m => m.Email == email).FirstOrDefault(); //checking if the emailid already exits for any user
            
            if (userWithSameEmail!=null)
            {
                userWithSameEmail.Password = password;
                databaseContext.Entry(userWithSameEmail).State = EntityState.Modified;
                databaseContext.SaveChanges();
                return RedirectToAction("LoginRenter");

            }
            return RedirectToAction("LoginRenter");

        }

        [HttpPost]
        public ActionResult Reset_Password(string password, Tenant tenant)
        {
            String email = Convert.ToString(Session["email"]);
            Database1Entities1 databaseContext = new Database1Entities1();
            var userWithSameEmail = databaseContext.Tenants.Where
                       (m => m.Email == email).FirstOrDefault(); //checking if the emailid already exits for any user

            if (userWithSameEmail != null)
            {
                userWithSameEmail.Password = password;
                databaseContext.Entry(userWithSameEmail).State = EntityState.Modified;
                databaseContext.SaveChanges();
                return RedirectToAction("LoginTenant");

            }
            return RedirectToAction("LoginTenant");

        }

        public ActionResult Mail()
        {
            return View();
        }



        //The form's data in Register view is posted to this method.
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetails(RegisterRenter registerDetails,HttpPostedFileBase image)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework
                using (var databaseContext = new Database1Entities1())
                {

                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    Renter reglog = new Renter();
                    var userWithSameEmail = databaseContext.Renters.Where
                        (m => m.Email == registerDetails.Email).FirstOrDefault(); //checking if the emailid already exits for any user
                    //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                    //If user is present, then true is returned.
                    //Save all details in RegitserUser object
                    if (userWithSameEmail == null)
                    {
                        
                        reglog.FirstName = registerDetails.FirstName;
                        reglog.LastName = registerDetails.LastName;
                        reglog.Email = registerDetails.Email;
                        reglog.Password = registerDetails.Password;
                        reglog.PhoneNumber = registerDetails.PhoneNumber;
                        reglog.AadharNumber = registerDetails.AadharNumber;
                        
                        if (image != null)
                        {
                            reglog.ProfileImage = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.ProfileImage, 0, image.ContentLength);
                        }

                        try
                        {
                            databaseContext.Renters.Add(reglog);
                            databaseContext.SaveChanges();


                            ViewBag.saved = "User Details Saved";
                            return View("LoginRenter");
                        }
                        catch (NullReferenceException ex)
                        {
                            ViewBag.Message = "Something Wrong Please Try Again";
                            return View("RegisterRenter");
                        }

                       
                       
                    }
                    else
                    {
                        ViewBag.Message = "Email already exists";

                        return View("RegisterRenter");
                    }
                }

            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("RegisterRenter", registerDetails);
            }
        }


        [HttpPost]
        public ActionResult SaveRegisterTenant(RegisterTenant tenantRegisterDetails, HttpPostedFileBase image)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework
                using (var databaseContext = new Database1Entities1())
                {

                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    Tenant reglog = new Tenant();
                    var userWithSameEmail = databaseContext.Tenants.Where(m => m.Email == tenantRegisterDetails.Email).FirstOrDefault(); //checking if the emailid already exits for any user
                    //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                    //If user is present, then true is returned.
                    //Save all details in RegitserUser object
                    if (userWithSameEmail == null)
                    {
                        reglog.FirstName = tenantRegisterDetails.FirstName;
                        reglog.LastName = tenantRegisterDetails.LastName;
                        reglog.Email = tenantRegisterDetails.Email;
                        reglog.Password = tenantRegisterDetails.Password;
                        reglog.PhoneNumber = tenantRegisterDetails.PhoneNumber;
                        reglog.AadharNumber = tenantRegisterDetails.AadharNumber;

                        if (image != null)
                        {
                            reglog.ProfileImage = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.ProfileImage, 0, image.ContentLength);
                        }

                        try
                        {
                            databaseContext.Tenants.Add(reglog);
                            databaseContext.SaveChanges();
                            return View("LoginTenant");
                        }
                        catch (NullReferenceException ex)
                        {
                            ViewBag.Message = "Something Wrong Please Try Again";
                            return View("RegisterTenant");
                        }

                        
                    }
                    else
                    {
                        ViewBag.Message = "Email already exists";

                        return View("RegisterTenant");
                    }
                }

            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("RegisterTenant", tenantRegisterDetails);
            }
        }



        public ActionResult LoginRenter()
        {
            return View();
        }

      



        //The login form is posted to this method.
        [HttpPost]
        public ActionResult LoginRenter(LoginRenter model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Email, false);
                    
                        return RedirectToAction("Index", "Account");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ViewBag.ErrorMessage = "INVALID LOGIN CREDENTIALS";
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }



        //function to check if User is valid or not
        public Renter IsValidUser(LoginRenter model)
        {
            using (var dataContext = new Database1Entities1())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                Renter user = dataContext.Renters.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                {
                   // HttpContext.Session.SetString("SellerID",UserName.ToString());
                    return user;
                   // Session["LoggedInUserDetail"] = new LoggedInUserDetail(user);
                }

            }
        }



        [HttpPost]
        public ActionResult LoginTenant(LoginTenant model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidTenant(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("IndexTenant","Account");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ViewBag.ErrorMessage = "Sorry, Please check your mail ID and password";
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }






        //function to check if User is valid or not


        public Tenant IsValidTenant(LoginTenant model)
        {
            using (var dataContext = new Database1Entities1())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                Tenant user = dataContext.Tenants.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;

                //If user is not present false is returned.
                else
                    return user;
            }
        }



      




        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("LoginRenter","Account");
        }

       

    }

}
