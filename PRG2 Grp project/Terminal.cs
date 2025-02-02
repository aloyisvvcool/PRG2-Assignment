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
    class Terminal
    {
        private string terminalName;

        public string TerminalName
        {
            get { return terminalName; }
            set { terminalName = value; }
        }

        private Dictionary<string, Airline> airlines;

        public Dictionary<string, Airline> Airlines
        {
            get { return airlines; }
            set { airlines = value; }
        }

        private Dictionary<string, Flight> flights;

        public Dictionary<string, Flight> Flights
        {
            get { return flights; }
            set { flights = value; }
        }

        private Dictionary<string, BoardingGate> boardingGates;

        public Dictionary<string, BoardingGate> BoardingGates
        {
            get { return boardingGates; }
            set { boardingGates = value; }
        }

        private Dictionary<string, double> gateFees;

        public Dictionary<string, double> GateFees
        {
            get { return gateFees; }
            set { gateFees = value; }
        }

        //Constructor
        public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
        {
            TerminalName = terminalName;
            Airlines = airlines;
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

        public void PrintAirlineFees()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine($"Airline Fees for {TerminalName}");
            Console.WriteLine("=============================================");
            Console.WriteLine($"{"Airline",-18}: {"Subtotal",-10} {"Discount",-10} {"Total",-10}");
            // Assign each flight in terminalFive.Flights to its corresponding airline in terminalFive.Airlines
            foreach (var flight in flights.Values)
            {
                // Extract airline code from the first two characters of the flight number
                string airlineCode = flight.FlightNumber.Substring(0, 2).ToUpper();

                // Find the airline using its code (we need to search since dictionary keys are full airline names)
                Airline correspondingAirline = airlines.Values.FirstOrDefault(a => a.Code == airlineCode); //Returns the name of the airline given airlineCode

                if (correspondingAirline != null)
                {
                    correspondingAirline.AddFlight(flight);
                }

            }

            double allFees = 0;
            double allDiscounts = 0;
            foreach (Airline airline in airlines.Values)
            {
                double totalFee = airline.CalculateFees(); // Use the existing methods in Airline.cs
                double totalDiscount = airline.CalculateDiscount();
                allFees += totalFee;
                allDiscounts += totalDiscount;
                Console.WriteLine($"{airline.Name,-18}: ${$"{totalFee + totalDiscount:F2}",-10}${$"{totalDiscount:F2}",-10}${$"{totalFee:F2}",-10}");
            }
            Console.WriteLine($"{"Total",-18}: ${$"{allFees + allDiscounts:F2}",-10}${$"{allDiscounts:F2}",-10}${$"{allFees:F2}",-10}");
        }

        public override string ToString()
        {
            return $"Terminal Name:{TerminalName}";
        }


    }
}
