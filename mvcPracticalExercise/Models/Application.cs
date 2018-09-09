using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcPracticalExercise.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public bool DUI { get; set; }
        public int SpeedingTickets { get; set; }
        public bool TypeOfCoverage { get; set; }
        public double QuoteAmount { get; set; }

    }
}