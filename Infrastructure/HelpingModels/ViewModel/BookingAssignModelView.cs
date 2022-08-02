using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class BookingAssignModelView
    {
        [Required]
        public int BookingId { get; set; }
        [Required]  
        [Range(1,int.MaxValue,ErrorMessage ="Please select agent!")]
        public int UserId { get; set; }
    }
}
