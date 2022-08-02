#region Using Statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace Infrastructure.HelpingModel
{
    public class FlightBooking
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string PNR { get; set; }
        public string AirlinePNR { get; set; }
        public string CCHolderName { get; set; }
        public int PortalId { get; set; }
        public int GDS { get; set; }
        public int ProviderId { get; set; }
        public int BookingType { get; set; }
        public int BookingSourceType { get; set; }
        public int BookingStatus { get; set; }
        public int BookingSubStatus { get; set; }
        public int TripType { get; set; }
        public string OriginCode { get; set; }
        public string DestinationCode { get; set; }
        public string Airline { get; set; }
        public DateTime? DeptDate { get; set; }
        public int AgentId { get; set; }
        public int AgentLead { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
    }
}
