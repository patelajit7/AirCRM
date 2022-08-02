using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{

    public class BillingDetails : ContentBase
    {
        public int BookingId { get; set; }
        public string CCHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CVVNumber { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public int CardType { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string BillingPhone { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPrimaryCard { get; set; }
        public string AreaCode { get; set; }
        public string CountryCode { get; set; }
    }

}
