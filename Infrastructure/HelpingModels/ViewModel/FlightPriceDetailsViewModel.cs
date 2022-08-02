using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels.ViewModel
{
    public class FlightPriceDetailsViewModel
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string FareBaseCode { get; set; }
        public TravellerPaxType PaxType { get; set; }
        public int Currency { get; set; }
        public int PaxCount { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Tax { get; set; }
        public decimal Markup { get; set; }
        public decimal SupplierFee { get; set; }
        public decimal Discount { get; set; }
        public bool IsSellInsurance { get; set; }
        public decimal InsuranceAmount { get; set; }
        public bool IsSellBaggageInsurance { get; set; }
        public decimal BaggageInsuranceAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsExtendedCancellation { get; set; }
        public decimal ExtendedCancellationAmount { get; set; }
        public decimal BookingFee { get; set; }
        public decimal GetTotal
        {
            get
            {
                return ((BaseFare + Markup + SupplierFee + BookingFee + Tax + (IsExtendedCancellation == true ? ExtendedCancellationAmount : 0) + (IsSellInsurance == true ? InsuranceAmount : 0) + (IsSellBaggageInsurance == true ? BaggageInsuranceAmount : 0)) - Discount) * PaxCount;
            }
        }
    }
}
