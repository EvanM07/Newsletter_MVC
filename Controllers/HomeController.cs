using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.MemoryMappedFiles;
using System.Data.SqlTypes;
using NewsLetter_MVC.Models;
using NewsLetter_MVC.ViewModels;

namespace NewsLetter_MVC.Controllers
{
    public class HomeController : Controller
    {
        public int SignupVM { get; private set; }

        public ActionResult Index()
        {
            return View();
        }
        /// WE ADD THE [HttpPost] TO THE BEGGING TO IDENTIFY THAT THIS IS A POST METHOD. 

        /// THIS IS ALL ABOUT THE FORM AND IF ALL THE FIELDS ARE COMPLETE AND NOT NULL THE ELSE STATEMENT WILL 
        /// TAKE THE USER VALUES AND ADD IT TO THE DATA BASE THAT WE CREATED.
        /// 
        /// THIS INCLUDES THE CONNECTION STRING, QUERYSTRING, TABLE NAMES, ALL OF THIS IS 
        /// WRAPPED IN A USING STATMENT THAT HAS PARAMETERS SO YOUR DATABASE DOESN'T
        /// GET CORRUPTED BY FOUL USER SQL INJECTION LIKE "DROP DATABASE".
        /// 
        /// THEN WE OPEN THE CONNECTION, INSERT THE VALUES INTO THE TABLE, AND THEN CLOSE THE CONNECITON. 
        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");

            }
            else
            {

                using (NewsletterEntities db = new NewsletterEntities())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;


                    db.SignUps.Add(signup);
                    db.SaveChanges();

                }

                return View("Success");
            }
        }
    }
}
    
