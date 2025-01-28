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
    public abstract class Flight : IComparable<Flight>
    {
        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        private string origin;

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }


        private string destination;

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }


        private DateTime expectedTime;

        public DateTime ExpectedTime
        {
            get { return expectedTime; }
            set { expectedTime = value; }
        }


        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        //Constructor
        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }


        //Methods
        public virtual double CalculateFees()
        {
            if (Origin == "Singapore (SIN)")
            {
                return 800;
            }
            else
            {
                return 500;
            }
        }

        //Icomparable
        public int CompareTo(Flight other)
        {

            return ExpectedTime.CompareTo(other.ExpectedTime);
        }


        //ToString
        public override string ToString()
        {
            return $"Flight Number:{FlightNumber} Origin:{Origin} Destination:{Destination} Expected Time:{ExpectedTime} Status:{Status} ";
        }

    }

    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
            : base(flightNumber, origin, destination, expectedTime, status) { }
    }

    class LWTTFlight : Flight
    {
        private readonly double requestFee = 500.00;
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
            : base(flightNumber, origin, destination, expectedTime, status) { }

        public override double CalculateFees()
        {
            return base.CalculateFees() + requestFee;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nRequest Fee: {requestFee}";
        }
    }

    class CFFTFlight : Flight
    {
        private readonly double requestFee = 150.00;
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
            : base(flightNumber, origin, destination, expectedTime, status) { }

        public override double CalculateFees()
        {
            return base.CalculateFees() + requestFee;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nRequest Fee: {requestFee}";
        }
    }

    class DDJBFlight : Flight
    {
        private readonly double requestFee = 300.00;

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
            : base(flightNumber, origin, destination, expectedTime, status) { }

        public override double CalculateFees()
        {
            return base.CalculateFees() + requestFee;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nRequest Fee: {requestFee}";
        }
    }
}
