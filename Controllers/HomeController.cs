using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.MemoryMappedFiles;
using System.Data.SqlTypes;


namespace NewsLetter_MVC.Controllers
{
    public class HomeController : Controller
    {
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
                string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}