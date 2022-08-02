using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class Bookings : ContentBase
    {
        public string Guid { get; set; }
        public string PNR { get; set; }
        public string AirlinePNR { get; set; }
        public string ReferenceNumber { get; set; }
        public int PortalId { get; set; }
        public int GDS { get; set; }
        public int ProviderId { get; set; }
        public int BookingType { get; set; }
        public int BookingSourceType { get; set; }
        public int BookingStatus { get; set; }
        public int BookingSubStatus { get; set; }
        public int AgentId { get; set; }
        public int AgentLead { get; set; }
        public int UserId { get; set; }
        public string ClientIP { get; set; }
        public bool IsBookingReso { get; set; }
        public string ResoMainDisp { get; set; }
        public decimal CurrencyConversion { get; set; }
        public int Currency { get; set; }
        public int ImportTransactionType { get; set; }
    }
}
