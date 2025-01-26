using System;
using System.Collections.Generic;
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
    class Terminal
    {
		private string terminalName;

		public string TerminalName
		{
			get { return terminalName; }
			set { terminalName = value; }
		}

		private Dictionary<string,Airline> airlines;

		public Dictionary<string,Airline> Airlines
		{
			get { return airlines; }
			set { airlines = value; }
		}

		private Dictionary<string,Flight> flights;

		public Dictionary<string,Flight> Flights
		{
			get { return flights; }
			set { flights = value; }
		}

		private Dictionary<string,BoardingGate> boardingGates;

		public Dictionary<string,BoardingGate> BoardingGates
		{
			get { return boardingGates; }
			set { boardingGates = value; }
		}

		private Dictionary<string,double> gateFees;

		public Dictionary<string,double> GateFees
		{
			get { return gateFees; }
			set { gateFees = value; }
		}

		//Constructor
		public Terminal(string terminalName, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
		{
			TerminalName = terminalName;
			Flights = flights;
			BoardingGates = boardingGates;
			GateFees = gateFees;
		}

		//Methods
		public bool AddAirline(Airline airline)
		{
			Airlines.Add(airline.Name, airline);
			return true;
		}

		public bool AddBoardingGate(BoardingGate gate)
		{
			BoardingGates.Add(gate.GateName, gate);
			return true;
		}

		public Airline GetAirlineFromFlight(Flight flight)
		{
			//Use first two characters of FlightNumber 
			foreach (Airline airline in Airlines.Values)
			{
				if (flight.FlightNumber.Substring(0,2) == airline.Code)
				{
					return airline;
				}
			}

			//If no airline is found
			return null;
		}

		public void PrintAirlineFees()
		{
			Console.WriteLine("Airline Fees:");
			foreach (Airline airline in Airlines.Values)
			{
				Console.WriteLine($"{airline.Name}: {airline.CalculateFees() + (airline.Flights.Values.Count * 300)}"); //airline.Flights.Values.Count * 300 to account for boarding gate fees
            }
		}

        public override string ToString()
        {
			return $"Terminal Name:{TerminalName}";
        }
		//INCOMPLETE


    }
}
