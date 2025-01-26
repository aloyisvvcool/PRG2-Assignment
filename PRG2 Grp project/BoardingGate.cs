using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Grp_project
{
    class BoardingGate
    {
		private string gateName;

		public string GateName
		{
			get { return gateName; }
			set { gateName = value; }
		}


		private bool supportsCFFT;

		public bool SupportsCFFT
		{
			get { return supportsCFFT; }
			set { supportsCFFT = value; }
		}


		private bool supportsDDJB;

		public bool SupportsDDJB
        {
			get { return supportsDDJB; }
			set { supportsDDJB = value; }
		}


		private bool supportsLWTT;

		public bool SupportsLWTT
        {
			get { return supportsLWTT; }
			set { supportsLWTT = value; }
		}


		private Flight flight;

		public Flight Flight
		{
			get { return flight; }
			set { flight = value; }
		}


		//Constructor
		public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
		{
			GateName = gateName;
			SupportsCFFT = supportsCFFT;
			SupportsDDJB = supportsDDJB;
			SupportsLWTT = supportsLWTT;
			Flight = flight;
		}

		//Methods
		public double CalculateFees()
		{
			//initialise fee
			double fees = 0;

			//IDK WHAT TO CALCULATE
			return 1.0;

        }

        public override string ToString()
        {
			return $"Gate Name:{GateName} SupportsCFFT:{SupportsCFFT} SupportsDDJB:{SupportsDDJB} SupportsLWTT:{SupportsLWTT} Flight:{Flight}";

		}




    }
}
