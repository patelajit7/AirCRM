using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class FlightSearchReport
    {
        public int Id { get; set; }
        public string GuidId { get; set; }
        public int PortalId { get; set; }
        public int AffiliateId { get; set; }
        public int TripType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime? Return { get; set; }
        public int Adult { get; set; }
        public int Senior { get; set; }
        public int Child { get; set; }
        public int InfantOnSeat { get; set; }
        public int InfantOnLap { get; set; }
        public int Cabin { get; set; }
        public int ResultFound { get; set; }
        public DateTime Created { get; set; }

    }
}
