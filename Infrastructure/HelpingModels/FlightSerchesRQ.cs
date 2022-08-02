using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class FlightSearchesRQ
    {
        public int PortalId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool WithResult { get; set; }
    }

    public class RequestedItineraryRQ
    {
        public int PortalId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

}
