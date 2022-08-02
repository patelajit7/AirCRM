using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class TravellersViewModel
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int PaxOrder { get; set; }
        public TravellerPaxType PaxType { get; set; }
        public TravellerTitleType Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string AirlineConfirmationNo { get; set; }
        public string TicketNo { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public string PassportNumber { get; set; }
        public DateTime PassportExpireDate { get; set; }
        public string PassportIssuedBy { get; set; }
        public string Email { get; set; }
        public string MealPreference { get; set; }
        public string SpecialPreference { get; set; }
    }
}
