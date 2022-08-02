using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class UpdateBookingStatusDetails
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        //[Range(1, 4, ErrorMessage = "Please select status!")]
        public BookingStatus BookingStatus { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Remarks can have max 500 character long!")]
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ReferenceId { get; set; }

    }
}
