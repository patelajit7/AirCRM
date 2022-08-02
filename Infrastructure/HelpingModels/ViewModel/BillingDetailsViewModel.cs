using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class BillingDetailsViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int BookingId { get; set; }
        [StringLength(100, ErrorMessage = "Card holder name max can be 100 character!")]
        public string CCHolderName { get; set; }
        [StringLength(20, ErrorMessage = "Card number max can be 20 digit!")]
        public string CardNumber { get; set; }
        [StringLength(5, ErrorMessage = "CVV number max can be 5 digit!", MinimumLength =3)]
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
