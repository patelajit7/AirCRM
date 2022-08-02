using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class BookingsViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter PNR!")]
        [StringLength(10, ErrorMessage = "Please enter valid PNR!", MinimumLength = 6)]
        public string PNR { get; set; }
        [StringLength(10, ErrorMessage = "Please enter valid AirlinePNR!", MinimumLength = 6)]
        public string AirlinePNR { get; set; }
        [Required(ErrorMessage ="Please select portal!")]
        [Range(1000,10000,ErrorMessage ="Invalid portal!")]
        public int PortalId { get; set; }
        [Required(ErrorMessage = "Please select provider!")]
        [Range(1, 6, ErrorMessage = "Invalid provider!")]
        public int ProviderId { get; set; }
        [Required(ErrorMessage = "Please select booking source!")]
        public int BookingSourceType { get; set; }
    }
}
