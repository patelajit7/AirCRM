using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class CouponSearchParameter
    {
        public int PortalId { get; set; }        
        public bool SkipFlag { get; set; }
        public string CommType { get; set; }
    }
}
