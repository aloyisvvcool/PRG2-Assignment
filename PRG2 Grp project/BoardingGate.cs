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
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight = null)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees()
        {
            return Flight != null ? Flight.CalculateFees() + 300 : 0;
        }

        public override string ToString()
        {
            return $"Gate Name: {GateName} SupportsCFFT: {SupportsCFFT} SupportsDDJB: {SupportsDDJB} SupportsLWTT: {SupportsLWTT} Flight: {Flight}";
        }
    }
}
