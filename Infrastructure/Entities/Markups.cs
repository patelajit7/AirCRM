using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class Markups : ContentBase
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Airline { get; set; }
        public int? TripType { get; set; }
        public string CabinType { get; set; }
        public string AirlineClass { get; set; }
        public string MarketingMedium { get; set; }
        public DateTime? DepartureStartDate { get; set; }
        public DateTime? DepartureEndDate { get; set; }
        public DateTime? ArrivalStartDate { get; set; }
        public DateTime? ArrivalEndDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string MarkupDays { get; set; }
        public TimeSpan? MarkupStartTime { get; set; }
        public TimeSpan? MarkupEndTime { get; set; }
        public int MarkupType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public int? FareType { get; set; }
        public int PortalId { get; set; }
        public string CreatedBy { get; set; }
        public string MarkupRoutes { get; set; }
        public bool IsLive { get; set; }
        public bool IsActive { get; set; }
        public string ProviderType { get; set; }
        public string DestCountry { get; set; }
    }

}
