using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class SearchCriteria
    {
        public DateTime From { get; set; }
        public DateTime End { get; set; }
        public int Status { get; set; }
        public BookingSearchType SearchType { get; set; }
        public bool IsOnlineBookings { get; set; }
    }
}
