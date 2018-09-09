using mvcPracticalExercise.Models;
using mvcPracticalExercise.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcPracticalExercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=mvcPracticalExercise;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Application(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear, string carMake, string carModel,
            bool dUI, int speedingTickets, bool typeOfCoverage)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                int userAge = 0;
                userAge = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear) userAge = userAge - 1;

                // quote

                double quoteAmount = 50;
                if ((userAge < 25 && userAge > 18) || userAge > 100) quoteAmount += 25;
                if (userAge < 18) quoteAmount += 100;
                if (carYear < 2000 || carYear > 2015) quoteAmount += 25;
                if (carMake == "Porsche" || carMake == "porsche") quoteAmount += 25;
                if ((carMake == "Porsche" || carMake == "porsche") && (carModel == "911 Carrera" || carModel == "911 carrera")) quoteAmount += 25;
                if (speedingTickets > 0)
                {
                    quoteAmount += (speedingTickets * 10);

                }
                if (dUI == true) quoteAmount += (quoteAmount * 0.25);
                if (typeOfCoverage == true) quoteAmount += (quoteAmount * 0.5);












                //input data into database

                string queryString = @"INSERT INTO Application (FirstName, LastName, EmailAddress, DateOfBirth, CarYear, CarMake, CarModel, DUI, SpeedingTickets, TypeOfCoverage, QuoteAmount) VALUES
                                        (@FirstName, @LastName, @EmailAddress, @DateOfBirth, @CarYear, @CarMake, @CarModel, @DUI, @SpeedingTickets, @TypeOfCoverage, @QuoteAmount)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
                    command.Parameters.Add("@CarYear", SqlDbType.Int);
                    command.Parameters.Add("@CarMake", SqlDbType.VarChar);
                    command.Parameters.Add("@CarModel", SqlDbType.VarChar);
                    command.Parameters.Add("@DUI", SqlDbType.VarChar);
                    command.Parameters.Add("@SpeedingTickets", SqlDbType.Int);
                    command.Parameters.Add("@TypeOfCoverage", SqlDbType.VarChar);
                    command.Parameters.Add("@QuoteAmount", SqlDbType.Float);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;
                    command.Parameters["@DateOfBirth"].Value = dateOfBirth;
                    command.Parameters["@CarYear"].Value = carYear;
                    command.Parameters["@CarMake"].Value = carMake;
                    command.Parameters["@CarModel"].Value = carModel;
                    command.Parameters["@DUI"].Value = dUI;
                    command.Parameters["@SpeedingTickets"].Value = speedingTickets;
                    command.Parameters["@TypeOfCoverage"].Value = typeOfCoverage;
                    command.Parameters["@QuoteAmount"].Value = quoteAmount;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();


                }
                ViewBag.quoteAmount = quoteAmount;

                return View("Success");
            }
        }


        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, DateOfBirth, CarYear, CarMake, CarModel, DUI, SpeedingTickets, TypeOfCoverage, QuoteAmount from Application";
            List<Application> quotes = new List<Application>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var quote = new Application();
                    quote.Id = Convert.ToInt32(reader["Id"]);
                    quote.FirstName = reader["FirstName"].ToString();
                    quote.LastName = reader["LastName"].ToString();
                    quote.EmailAddress = reader["EmailAddress"].ToString();
                    quote.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    quote.CarYear = Convert.ToInt32(reader["CarYear"]);
                    quote.CarMake = reader["CarMake"].ToString();
                    quote.CarModel = reader["CarModel"].ToString();
                    quote.DUI = Convert.ToBoolean(reader["DUI"]);
                    quote.SpeedingTickets = Convert.ToInt32(reader["SpeedingTickets"]);
                    quote.TypeOfCoverage = Convert.ToBoolean(reader["TypeOfCoverage"]);
                    quote.QuoteAmount = Convert.ToDouble(reader["QuoteAmount"]);
                    quotes.Add(quote);

                }
            }
            var quoteVMs = new List<QuoteVM>();
            foreach (var quote in quotes)
            {
                var quoteVM = new QuoteVM();
                quoteVM.FirstName = quote.FirstName;
                quoteVM.LastName = quote.LastName;
                quoteVM.EmailAddress = quote.EmailAddress;
                quoteVM.QuoteAmount = quote.QuoteAmount;
                quoteVMs.Add(quoteVM);
            }
            return View(quoteVMs);
        }
    }
}