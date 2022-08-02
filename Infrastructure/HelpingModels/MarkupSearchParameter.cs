using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class MarkupSearchParameter
    {
        public int PortalId { get; set; }
        public string Airline { get; set; }
        public string DestCountry { get; set; }
        public string ProviderType { get; set; }
        public string Routes { get; set; }
        public TripType TripType { get; set; }
        public string MarketingMedium { get; set; }
        public bool SkipFlag { get; set; }
        public string CommType { get; set; }
    }
}
