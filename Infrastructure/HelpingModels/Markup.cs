using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{

    public class Markup
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Airline { get; set; }
        public int? TripType { get; set; }
        public string CabinType { get; set; }
        public string AirlineClass { get; set; }
        public string MarketingMedium { get; set; }
        public string DepartureStartDate { get; set; }
        public string DepartureEndDate { get; set; }
        public string ArrivalStartDate { get; set; }
        public string ArrivalEndDate { get; set; }
        public string ExpiryDate { get; set; }
        public string MarkupDays { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        [DataType(DataType.Time)]
        public TimeSpan? MarkupStartTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        [DataType(DataType.Time)]
        public TimeSpan? MarkupEndTime { get; set; }
        public int MarkupType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public int? FareType { get; set; }
        public int PortalId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public string MarkupRoutes { get; set; }
        public string ProviderType { get; set; }
        public string DestCountry { get; set; }
    }
}
