using NewsLetter_MVC.Models;
using NewsLetter_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsLetter_MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var singups = db.SignUps.Where(x => x.Removed == null).ToList();
                var signupVMs = new List<SignUpVM>();
                foreach (var signup in singups)
                {
                    var signupVM = new SignUpVM();
                    signupVM.Id = signup.Id;
                    signup.FirstName = signup.FirstName;
                    signup.LastName = signup.LastName;
                    signup.EmailAddress = signup.EmailAddress;
                    signupVMs.Add(signupVM);
                }

                return View(signupVMs);
            }
        }
        public ActionResult Unsubscribe(int Id)
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signup = db.SignUps.Find(Id);
                signup.Removed = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
    
