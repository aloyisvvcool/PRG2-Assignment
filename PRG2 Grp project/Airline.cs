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

//NO ERROR HANDLING YET
namespace PRG2_Grp_project
{
    class Airline
    {
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}


		private string code;

		public string Code
		{
			get { return code; }
			set { code = value; }
		}


		private Dictionary<string,Flight> flights;

		public Dictionary<string, Flight> Flights
		{
			get { return flights; }
			set { flights = value; }
		}

		//Constructor
		public Airline(string name, string code, Dictionary<string,Flight> flights)
		{
			Name = name;
			Code = code;
			Flights = flights;
		}


		//Methods
		public bool AddFlight(Flight flight)
		{
			//NOT COMPLETE YET
			flights.Add(flight.FlightNumber, flight);
			return true;
		}

		public bool RemoveFlight(Flight flight)
		{
			foreach (Flight f in flights.Values)
			{
				if (flight == f)
				{
					flights.Remove(f.FlightNumber);
					return true;
				}
			}
			return false;
		}

        public double CalculateFees()
        {
            //Initialise Fee
            double fees = 0;
            //Calculate Fee before discounts.
            foreach (Flight f in flights.Values)
            {
                fees += f.CalculateFees();
            }

            int threeFlightDiscountNumber = (Flights.Count / 3);

            //Discount for multiple flights
            if (flights.Count() > 5)
            {
                fees = fees * 0.97;
            }

            //Three Flight discount stackable
            fees -= 350 * threeFlightDiscountNumber;

            //initalise 9pm and 11am
            DateTime startTime = DateTime.Today.AddHours(21); // 9:00 PM 
            DateTime endTime = DateTime.Today.AddHours(11); // 11:00 AM  

            foreach (Flight f in flights.Values)
            {
                //Time discounts
                if (f.ExpectedTime > startTime || f.ExpectedTime < endTime)
                {
                    fees -= 110;
                }

                //Flight Origin discounts
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                {
                    fees -= 25;
                }
                //No Special Request Code discounts
                if (f is NORMFlight)
                {
                    fees -= 50;
                }
            }


            return fees - f.CalculateFees();
        }



        public override string ToString()
        {
			return $"Name:{Name} Code:{Code}";
        }
    }
}
