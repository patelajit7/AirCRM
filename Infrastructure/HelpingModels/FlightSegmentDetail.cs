using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class FlightSegmentDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int SegmentOrder { get; set; }
        public string FlightNumber { get; set; }
        public bool IsReturn { get; set; }
        public string OperatingCode { get; set; }
        public string MarketingCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DeptDateTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string DeptTerminal { get; set; }
        public string ArrivalTerminal { get; set; }
        public string EquipmentDetail { get; set; }
        public string SegmentClass { get; set; }
        public int Stops { get; set; }
        public int Cabin { get; set; }
        public string CompanyFranchiseDetails { get; set; }
        public string TechnicalStoppages { get; set; }
        public string AirlineLocator { get; set; }
        public string RelatedProductStatus { get; set; }
    }

    public class FlightSegmentDetailToUpdate
    {
        public List<FlightSegmentDetail> Segments { get; set; }
    }
}
