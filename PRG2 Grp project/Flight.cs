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
    abstract class Flight : IComparable<Flight>
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
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
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
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {

        }

        public override double CalculateFees()
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

        public override string ToString()
        {
            return base.ToString();
        }
    }

    class LWTTFlight : Flight
    {
        private double requestFee;

        public double RequestFee
        {
            get { return requestFee; }
            set { requestFee = value; }
        }

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override string ToString()
        {
            return base.ToString() + $"Request Fee:{requestFee}";
        }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)")
            {
                return 800 + 500;
            }
            else
            {
                return 500 + 500;
            }
        }

    }

    class DDJBFlight : Flight
    {
        private double requestFee;

        public double RequestFee
        {
            get { return requestFee; }
            set { requestFee = value; }
        }

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)")
            {
                return 800 + 300;
            }
            else
            {
                return 500 + 300;
            }
        }

        public override string ToString()
        {
            return base.ToString() + $"Request Fee:{requestFee}";
        }

    }
    class CFFTFlight : Flight
    {
        private double requestFee;

        public double RequestFee
        {
            get { return requestFee; }
            set { requestFee = value; }
        }

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            if (Origin == "Singapore (SIN)")
            {
                return 800 + 150;
            }
            else
            {
                return 500 + 150;
            }
        }

        public override string ToString()
        {
            return base.ToString() + $"Request Fee:{requestFee}";
        }
    }
}
