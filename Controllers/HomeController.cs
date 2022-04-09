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
        private readonly string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

                /// THIS IS USING ADO.NET 

                string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress) VALUES
                                        (@FirstName, @LastName, @EmailAddress)";

                
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return View("Success");
            }
        }

            /// WE ARE MAKING A METHOD CALLED ADMIN THIS WILL RETURN THE 
            /// VIEW(); WHICH WILL BE THE PAGE THAT THE ADMIN WILL HAVE ACCESS TO. 
        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress SocialSecurityNumber from Signups";

            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var signup = new NewsletterSignUp();
                    signup.Id = Convert.ToInt32(reader["Id"]);
                    signup.FirstName = reader["FirstName"].ToString();
                    signup.LastName = reader["LastName"].ToString();
                    signup.EmailAddress = reader["EmailAddress"].ToString();
                    signup.SocialSecurityNumber = reader["SocialSecurityNumber"].ToString();

                    signups.Add(signup);
                }
            }
            var  signupVMs= new List<SignUpVM>();
            foreach (var signup in signups)
            {
                var signupVM = new SignUpVM();
                signup.FirstName = signup.FirstName;
                signup.LastName = signup.LastName;
                signup.EmailAddress = signup.EmailAddress;
                signupVMs.Add(signupVM);
            }
            return View (signupVMs);
        }
    }
}