using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class CouponMaster
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public string CouponLabel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; }
        public int PortalId { get; set; }
        public bool IsActive { get; set; }
        public bool IsLive { get; set; }
        public bool IsDefault { get; set; }
    }
}
