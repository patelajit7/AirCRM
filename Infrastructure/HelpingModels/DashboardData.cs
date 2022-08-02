using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class DashboardData
    {
        public int NewBooking { get; set; }
        public int Inprogress { get; set; }
        public int Completed { get; set; }
        public int TotalBooking { get; set; }
        public MonthBooking MonthBooking { get; set; }
        public MonthSearch MonthSearch { get; set; }
        public RouteSearch RouteSearch { get; set; }
        public AffiliateSearches AffiliateSearches { get; set; }

    }
    public class MonthBooking
    {
        public List<DayCount> Data { get; set; }
    }
    public class MonthSearch
    {
        public List<DayCount> Data { get; set; }
    }
    public class RouteSearch
    {
        public List<RouteCount> Data { get; set; }
    }

    public class DayCount
    {
        public int Day { get; set; }
        public int Count { get; set; }
    }
    public class RouteCount
    {
        public string Route { get; set; }
        public int Count { get; set; }
    }

    public class AffiliateSearches
    {
        public List<AffiliateSearch> Affiliates { get; set; }
    }

    public class AffiliateSearch
    {
        public AffiliateType Afffliate { get; set; }
        public int Count { get; set; }
    }

    public class Dataset
    {
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
    }

    public class PieChartData
    {
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }



    public class BarchartDataset
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool pointRadius { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public List<int> data { get; set; }
    }

    public class BarChartData
    {
        public List<string> labels { get; set; }
        public List<BarchartDataset> datasets { get; set; }
    }
}
