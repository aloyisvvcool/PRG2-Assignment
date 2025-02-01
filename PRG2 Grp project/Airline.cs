using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//==========================================================
// Student Number : S10267956
// Student Name : Samuel Ng En Yi
// Partner Name : Aloysius Luke Tay Shi Yuan
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

            foreach (Flight f in flights.Values)
            {
                fees += f.CalculateFees();
            }

            int threeFlightDiscountNumber = flights.Count / 3;
            if (flights.Count > 5)
            {
                fees *= 0.97;
            }
            fees -= 350 * threeFlightDiscountNumber;

            DateTime startTime = DateTime.Today.AddHours(21);
            DateTime endTime = DateTime.Today.AddHours(11);

            foreach (Flight f in flights.Values)
            {
                if (f.ExpectedTime > startTime || f.ExpectedTime < endTime)
                {
                    fees -= 110;
                }
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                {
                    fees -= 25;
                }
                if (f is NORMFlight)
                {
                    fees -= 50;
                }
            }
            fees += flights.Values.Count * 300;
            return fees;
        }

        public override string ToString()
        {
            return $"Name:{Name} Code:{Code}";
        }
    }
}
