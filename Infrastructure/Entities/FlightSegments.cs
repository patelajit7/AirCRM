using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class FlightSegments : ContentBase
    {
        public int BookingId { get; set; }
        public int SegmentOrder { get; set; }
        public bool IsReturn { get; set; }
        public string FlightNumber { get; set; }
        public string OptAirlineCode { get; set; }
        public string MktAirlineCode { get; set; }
        public string OriginCode { get; set; }
        public DateTime DeptDateTime { get; set; }
        public string DeptTerminal { get; set; }
        public string DestinationCode { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string ArrivalTerminal { get; set; }
        public string EquipmentDetail { get; set; }
        public string SegmentClass { get; set; }
        public int Stops { get; set; }
        public int Cabin { get; set; }
        public string CompanyFranchiseDetails { get; set; }
        public string TechnicalStoppages { get; set; }
        public string AirlineLocator { get; set; }
        public string SegmentType { get; set; }
    }
}
