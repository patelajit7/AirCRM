#region Using Statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace Infrastructure.HelpingModel
{
    public class FlightBookingsSearchRQ
    {

        public BookingSearchType SearchType { get; set; }

        public string SearchValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
