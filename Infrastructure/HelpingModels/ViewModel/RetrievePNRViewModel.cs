using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    
    public class RetrievePNRViewModel
    {
        [Required(ErrorMessage = "Please enter reference number")]
        [StringLength(80,ErrorMessage = "Invalid reference number")]
        public string ReferenceNumber { get; set; }
    }
}
