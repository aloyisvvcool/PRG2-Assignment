using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//==========================================================
// Student Number : S10266029
// Student Name : Aloysius Luke Tay Shi Yuan
// Partner Name : Samuel Ng En Yi
//==========================================================

namespace PRG2_Grp_project
{
    class Airline
    {
        private string name;
        public string Name { get { return name; } set { name = value; } }

        private string code;
        public string Code { get { return code; } set { code = value; } }

        private Dictionary<string, Flight> flights;
        public Dictionary<string, Flight> Flights { get { return flights; } set { flights = value; } }

        // Constructor
        public Airline(string name, string code, Dictionary<string, Flight> flights)
        {
            Name = name;
            Code = code;
            Flights = flights ?? new Dictionary<string, Flight>();
        }

        // Methods
        public bool AddFlight(Flight flight)
        {
            if (flights.ContainsKey(flight.FlightNumber))
            {
                Console.WriteLine("Error: Flight number already exists.");
                return false;
            }
            flights.Add(flight.FlightNumber, flight);
            return true;
        }

        public bool RemoveFlight(string flightNumber)
        {
            return flights.Remove(flightNumber);
        }

        public double CalculateFees()
        {
            double fees = 0;
            double discount = 0;

            foreach (Flight f in flights.Values)
            {
                fees += f.CalculateFees();
            }

            if (flights.Count > 5)
            {
                discount += fees * 0.03; //For each airline with more than 5 flights arriving/departing, the airline will receive an additional discount
            }

            discount += 350 * Math.Floor(Convert.ToDouble(flights.Count / 3)); //For every 3 flights arriving/departing, airlines will receive a discoun

            DateTime startTime = DateTime.Today.AddHours(21); //Represents 9PM
            DateTime endTime = DateTime.Today.AddHours(11); //Represents 11AM

            foreach (Flight f in flights.Values)
            {
                if (f.ExpectedTime.Hour > startTime.Hour || f.ExpectedTime.Hour < endTime.Hour)
                {
                    discount += 110; //For each flight arriving/ departing before 11am or after 9pm
                }
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                {
                    discount += 25; //For each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                }
                if (f is NORMFlight)
                {
                    discount += 50; //For each flight not indicating any Special Request Codes
                }
            }
            fees += flights.Values.Count * 300; //Boarding Gate fee
            return fees-discount;
        }
        public double CalculateDiscount()
        {
            double fees = 0;
            double discount = 0;

            foreach (Flight f in flights.Values)
            {
                fees += f.CalculateFees();
            }

            if (flights.Count > 5)
            {
                discount += fees * 0.03; //For each airline with more than 5 flights arriving/departing, the airline will receive an additional discount
            }

            discount += 350 * Math.Floor(Convert.ToDouble(flights.Count / 3)); //For every 3 flights arriving/departing, airlines will receive a discoun

            DateTime startTime = DateTime.Today.AddHours(21); //Represents 9PM
            DateTime endTime = DateTime.Today.AddHours(11); //Represents 11AM

            foreach (Flight f in flights.Values)
            {
                if (f.ExpectedTime.Hour > startTime.Hour || f.ExpectedTime.Hour < endTime.Hour)
                {
                    discount += 110; //For each flight arriving/ departing before 11am or after 9pm
                }
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                {
                    discount += 25; //For each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                }
                if (f is NORMFlight)
                {
                    discount += 50; //For each flight not indicating any Special Request Codes
                }
            }
            fees += flights.Values.Count * 300; //Boarding Gate fee
            return discount;
        }

        public override string ToString()
        {
            return $"Name:{Name} Code:{Code}";
        }
    }
}
