using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class BookingRemarks
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Remarks can have max 500 character long!")]
        public string Remarks { get; set; }       
    }
}
