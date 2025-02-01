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
    abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled")
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status ?? "Scheduled"; //Flight status defaults to Scheduled if no argument is given
        }

        public virtual double CalculateFees()
        {
            return Origin == "Singapore (SIN)" ? 800 : 500; //If the flight originates from Singapore (SIN), 800 is returned. Otherwise it is implied that arrives at Singapore (SIN) and 500 is returned.
        }

        public int CompareTo(Flight other)
        {
            return ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public override string ToString()
        {
            return $"{FlightNumber,-15} {Origin,-20} {Destination,-20} {ExpectedTime:dd/MM/yyyy h:mm:ss tt} {Status,-15}";
        }

    }

    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled")
            : base(flightNumber, origin, destination, expectedTime, status) { }

        public override double CalculateFees()
        {
            return base.CalculateFees(); // Normal flight has standard fees
        }
    }

    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
            : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + 500; // LWTT Fee
        }
    }

    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
            : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + 300; // DDJB Fee
        }
    }

    class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee)
            : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + 150; // CFFT Fee
        }
    }

}
