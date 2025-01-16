using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Grp_project
{
    abstract class Flight:IComparable<Flight>
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
		public abstract double CalculateFees();//Abstract Method

		//Icomparable
		public int CompareTo(Flight other)
		{
			
			return ExpectedTime.CompareTo(other.ExpectedTime);
        }


		//NOT COMPLETED YET
        public override string ToString()
        {
			return base.ToString();
        }

    }

	class NORMFlight:Flight
	{
		public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status):base(flightNumber,origin,destination,expectedTime,status) 
		{
			
		}

		public override double CalculateFees()
		{
			return 1.0;
		}

    }

    class LWTTFlight : Flight
    {
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {

        }

        public override double CalculateFees()
        {
            return 1.0;
        }

    }

    class DDJBFlight : Flight
    {
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {

        }

        public override double CalculateFees()
        {
            return 1.0;
        }

    }
    class CFFTFlight : Flight
    {
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {

        }

        public override double CalculateFees()
        {
            return 1.0;
        }

    }
}
