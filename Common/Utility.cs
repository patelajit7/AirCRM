#region Using Statement
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Reflection;
using System.ComponentModel;
using Configuration;
using Infrastructure.Interfaces;
using Logger;
using System.Runtime.Caching;
using System.Configuration;
using Database;
using Infrastructure;
using System.Globalization;
using Infrastructure.Entities;
using Infrastructure.HelpingModels;
using Configration;
using System.Data.SqlClient;
#endregion
namespace Common
{
    public class Utility
    {

        public static Settings Settings { get; set; }
        public static PortalSettings PortalSettings { get; set; }
        public static ILoggingService Logger = (ILoggingService)LoggingService.GetLoggingService();
        private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        public static IDatabase DatabaseService = null;
        private static readonly object SyncPoolRoot = new Object();
        public static List<Airports> Airports { get; set; }
        public static List<string> MultiAirportCityCode { get; set; }
        public static List<Airlines> Airlines { get; set; }
        public static List<UserData> Users { get; set; }
        private static bool IsApplicationLoaded { get; set; }
        private static bool IsSettingsLoaded { get; set; }
        public static MemoryCache GLobalCache = MemoryCache.Default;
        //public static IMongoDBRepository MongoInstance = null;
        //public static ICachingProvider CachingProvider = null;
        public static string ConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int NewBookingCount { get; set; } = 0;
        public static List<Currency> Currencies { get; set; }
        public static void LoadApplicationConfiguration(System.Web.HttpContext httpContext)
        {
            try
            {
                Logger.Info("WinTracking START LOADING");
                if (!IsApplicationLoaded)
                {
                    if (!IsApplicationLoaded)
                    {
                        lock (SyncPoolRoot)
                        {
                            if (!IsApplicationLoaded)
                            {
                                if (IsLoadConfig())
                                {
                                    DatabaseService = (IDatabase)new DatabaseService(Utility.ConnString);
                                    IsApplicationLoaded = true;
                                    List<Task> lstTasks = new List<Task>
                                                        {
                                                            Task.Factory.StartNew(()=>Utility.Airports=Utility.DatabaseService.List<Airports>()),
                                                            Task.Factory.StartNew(()=>Utility.Airlines=Utility.DatabaseService.List<Airlines>()),
                                                            Task.Factory.StartNew(()=>Utility.Users=Utility.GetUsers(null)),
                                                            Task.Factory.StartNew(()=>Utility.Currencies=Utility.FetchCurrencyFromDB())
                                                        };
                                    Task.WaitAll(lstTasks.ToArray());
                                }
                            }
                        }
                    }
                }

                Logger.Info("WinTracking END LOADING");
            }
            catch (Exception ex)
            {
                Logger.Error("WinTracking START ISSUE:EXCEPTION|" + ex.ToString());
            }
        }
        private static bool LoadPortalConfigration()
        {
            bool isSuccess = false;
            try
            {
                Utility.Logger.Info("PortalSettings|BEGIN");
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, string.Format("Configuration\\Portal.config"));
                StreamReader reader = new StreamReader(path);
                Utility.PortalSettings = Utility.GetFileDeserialize<PortalSettings>(reader);
                isSuccess = true;
                Utility.Logger.Info("PortalSettings|END");
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.LoadPortalConfigration" + ex.ToString());
            }
            return isSuccess;
        }
        private static bool IsLoadConfig()
        {
            try
            {
                Logger.Debug("Common.Utility.IsLoadConfig:Begin");
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, string.Format("Configuration\\Settings.config"));
                StreamReader reader = new StreamReader(path);
                Settings = GetFileDeserialize<Settings>(reader);
                IsSettingsLoaded = LoadPortalConfigration();
                Utility.Logger.Info("Common.Utility.IsLoadConfig:Load Data Successfully.");
            }
            catch (Exception ex)
            {
                IsSettingsLoaded = false;
                Utility.Logger.Error("Common.Utility.IsLoadConfig:" + ex.ToString());
            }
            return IsSettingsLoaded;
        }

        public static T GetFileDeserialize<T>(StreamReader reader)
        {
            T response = default(T);
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                object obj = deserializer.Deserialize(reader);
                response = (T)obj;
                reader.Close();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetFileDeserialize<T>:" + ex.ToString());
            }
            return response;
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string enumDesc = string.Empty;

            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumDesc;
        }

        public static DateTime GetDateFromString(string date)
        {
            DateTime response = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    int[] arr = new int[3]; ;
                    if (date.Contains('/'))
                    {
                        arr = date.Split('/').Select(o => int.Parse(o)).ToArray();
                    }
                    else if (date.Contains('-'))
                    {
                        arr = date.Split('-').Select(o => int.Parse(o)).ToArray();
                    }

                    if (arr.Length == 3)
                    {
                        response = new DateTime(arr[2], arr[1], arr[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetDateFromString:" + ex.ToString());
            }
            return response;
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string Convert(long value)
        {
            TimeSpan timeSpan;
            timeSpan = TimeSpan.FromSeconds(value);
            if (timeSpan == TimeSpan.Zero)
            {
                return "00:00:00";
            }
            if (timeSpan.Days > 0)
            {
                return string.Format("{0:D3}:{1:D2}:{2:D2}", ((timeSpan.Days * 24) + timeSpan.Hours), timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }

        }

        public static bool IsCurrentDate(DateTime logDate)
        {
            bool response = false;
            try
            {
                DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime reportDate = new DateTime(logDate.Year, logDate.Month, logDate.Day);
                response = DateTime.Compare(currentDate, reportDate) == 0 ? true : false;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.IsCurrentDate:" + ex.ToString());
            }
            return response;
        }
        public static bool IsCurrentDate(DateTime logDate, DateTime statusDate)
        {
            bool response = false;
            try
            {
                DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime reportDate = new DateTime(logDate.Year, logDate.Month, logDate.Day);
                DateTime stDate = new DateTime(statusDate.Year, statusDate.Month, statusDate.Day);
                response = DateTime.Compare(currentDate, reportDate) == 0 && DateTime.Compare(currentDate, stDate) == 0 ? true : false;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.IsCurrentDate:" + ex.ToString());
            }
            return response;
        }
        public static DateTime GetUTCTime(DateTime sourceDatetime, BookingTimeZone sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(sourceDatetime, TimeZoneInfo.FindSystemTimeZoneById(Utility.GetEnumDescription(sourceTimeZone)));
        }
        //public static string GetTotalWorked(List<DateWiseData> dateWises)
        //{
        //    string response = "00:00:00";
        //    try
        //    {
        //        long total = dateWises.Sum(o => o.Login);
        //        response = Convert(total);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("Common.Utility.GetTotal:" + ex.ToString());
        //    }
        //    return response;
        //}
        //public static string GetTotalIdle(List<DateWiseData> dateWises)
        //{
        //    string response = "00:00:00";
        //    try
        //    {
        //        long total = dateWises.Sum(o => o.Idle);
        //        response = Convert(total);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("Common.Utility.GetTotal:" + ex.ToString());
        //    }
        //    return response;
        //}
        //public static string GetTotalLocked(List<DateWiseData> dateWises)
        //{
        //    string response = "00:00:00";
        //    try
        //    {
        //        long total = dateWises.Sum(o => o.Locked);
        //        response = Convert(total);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("Common.Utility.GetTotal:" + ex.ToString());
        //    }
        //    return response;
        //}

        //public static string ExportDatatableToHtml(DataTable dt)
        //{
        //    StringBuilder strHTMLBuilder = new StringBuilder();
        //    try
        //    {
        //        //strHTMLBuilder.Append("<html>");
        //        //strHTMLBuilder.Append("<head>");
        //        //strHTMLBuilder.Append("</head>");
        //        //strHTMLBuilder.Append("<body>");
        //        strHTMLBuilder.Append("<table style='font-family:arial, sans-serif;  border-collapse:collapse;width:100%'>");

        //        strHTMLBuilder.Append("<thead><tr>");
        //        foreach (DataColumn myColumn in dt.Columns)
        //        {
        //            strHTMLBuilder.Append("<th style='border:1px solid #dddddd;  text-align:left; padding:8px;'>");
        //            strHTMLBuilder.Append(myColumn.ColumnName);
        //            strHTMLBuilder.Append("</th>");

        //        }
        //        strHTMLBuilder.Append("</tr></thead>");

        //        foreach (DataRow myRow in dt.Rows)
        //        {
        //            strHTMLBuilder.Append("<tr>");
        //            foreach (DataColumn myColumn in dt.Columns)
        //            {
        //                strHTMLBuilder.Append("<td style='border:1px solid #dddddd;  text-align:left; padding:8px;'> ");
        //                strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
        //                strHTMLBuilder.Append("</td>");

        //            }
        //            strHTMLBuilder.Append("</tr>");
        //        }
        //        strHTMLBuilder.Append("</table>");
        //        //strHTMLBuilder.Append("</body>");
        //        //strHTMLBuilder.Append("</html>");
        //        string Htmltext = strHTMLBuilder.ToString();
        //        return Htmltext;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("Common.Utility.ExportDatatableToHtml:" + ex.ToString());
        //    }

        //    return "";
        //}
        public static string GetTitleCase(string text)
        {
            return textInfo.ToTitleCase(text);
        }
        public static Airports GetAirportDetails(string code, OriginType originType = OriginType.Airport)
        {
            Airports airports = null;
            try
            {
                switch (originType)
                {
                    case OriginType.Airport:
                        airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                        break;
                    case OriginType.City:
                        airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                        break;
                    default:
                        airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                        if (airports == null)
                        {
                            airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.Utility.GetAirportDetails<T>:" + ex.ToString());
            }
            return airports;
        }
        public static string GetAirportCity(string airport)
        {
            string response = string.Empty;
            try
            {
                Airports airports = GetAirportDetails(airport);
                if (airports != null)
                {
                    response = string.Format("{0}, {1}", airports.City, airports.CountryCode);
                }
                else
                {
                    response = airport;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.Utility.GetAirportInfo<T>:" + ex.ToString());
            }
            return response;
        }
        public static string GetCountryCode(string _code)
        {
            string response = string.Empty;
            Airports airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
            if (airports != null)
            {
                response = string.Format("{0}", airports.CountryCode);
            }
            else
            {
                airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                if (airports != null)
                {
                    response = string.Format("{0}", airports.CountryCode);
                }
            }
            return response;
        }

        public static string GetCountryName(string _code)
        {
            string response = string.Empty;
            Airports airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
            if (airports != null)
            {
                response = string.Format("{0}", airports.CountryName);
            }
            else
            {
                airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                if (airports != null)
                {
                    response = string.Format("{0}", airports.CountryName);
                }
            }
            return response;
        }

        public static string GetAirportName(string _code)
        {
            string response = string.Empty;
            Airports airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
            if (airports != null)
            {
                response = string.Format("{0}", airports.AirportName);
            }
            else
            {
                airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                if (airports != null)
                {
                    response = string.Format("{0}", airports.City);
                }
            }
            return response;
        }

        public static string GetCityName(string _code)
        {
            string response = string.Empty;
            Airports airports = Utility.Airports.Where<Airports>(o => o.AirportCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
            if (airports != null)
            {
                response = string.Format("{0}", airports.City);
            }
            else
            {
                airports = Utility.Airports.Where<Airports>(o => o.CityCode.Equals(_code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airports>();
                if (airports != null)
                {
                    response = string.Format("{0}", airports.City);
                }
            }
            return response;
        }
        public static string GetDuration(TimeSpan? _duration)
        {
            string response = string.Empty;
            try
            {
                if (_duration == null || _duration == TimeSpan.MinValue)
                {
                    response = "--";
                }
                else
                {
                    TimeSpan tempSpan = (_duration ?? TimeSpan.MinValue);
                    response = string.Format("{0}h {1}m", (int)tempSpan.TotalHours, tempSpan.Minutes);


                }
            }
            catch (Exception ex)
            {

                Utility.Logger.Error("EasyPro.GetDuration" + ex.ToString());
            }
            return response;
        }

        //public static TimeSpan GetTotalLayoverTime(List<Segments> segments)
        //{
        //    TimeSpan timeSpan = TimeSpan.MinValue;
        //    try
        //    {

        //        if (segments != null && segments.Count > 1)
        //        {
        //            for (int i = 0; i <= segments.Count - 1; i++)
        //            {
        //                if (i != 0)
        //                {
        //                    if (timeSpan == TimeSpan.MinValue)
        //                    {
        //                        timeSpan = new DateTime(segments[i].Departure.Year, segments[i].Departure.Month, segments[i].Departure.Day, segments[i].DepartureTime.Hours, segments[i].DepartureTime.Minutes, 0) - new DateTime(segments[i - 1].Arrival.Year, segments[i - 1].Arrival.Month, segments[i - 1].Arrival.Day, segments[i - 1].ArrivalTime.Hours, segments[i - 1].ArrivalTime.Minutes, 0);
        //                    }
        //                    else
        //                    {
        //                        timeSpan = timeSpan + (new DateTime(segments[i].Departure.Year, segments[i].Departure.Month, segments[i].Departure.Day, segments[i].DepartureTime.Hours, segments[i].DepartureTime.Minutes, 0) - new DateTime(segments[i - 1].Arrival.Year, segments[i - 1].Arrival.Month, segments[i - 1].Arrival.Day, segments[i - 1].ArrivalTime.Hours, segments[i - 1].ArrivalTime.Minutes, 0));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("GetLayoverTime" + ex.ToString());
        //    }
        //    return timeSpan;
        //}

        //public static int GetTotalStops(List<Segments> segments)
        //{
        //    int stop = 0;
        //    try
        //    {

        //        if (segments != null && segments.Count > 1)
        //        {
        //            stop = segments.Count - 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.Logger.Error("GetTotalStops" + ex.ToString());
        //    }
        //    return stop;
        //}
        public static string GetAilineName(string airlineCode)
        {
            string response = airlineCode;
            try
            {
                if (!string.IsNullOrEmpty(airlineCode))
                {
                    Airlines airline = Utility.Airlines.Where<Airlines>(o => o.Code.Equals(airlineCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault<Airlines>();
                    if (airline != null)
                    {
                        response = airline.Name;
                    }
                }
            }
            catch (Exception ex)
            {

                Utility.Logger.Error("Common.Utility.GetAilineName" + ex.ToString());
            }
            return response;
        }
        public static List<State> GetStates(string countryCode)
        {
            List<State> response = null;
            try
            {
                if (!string.IsNullOrEmpty(countryCode))
                {
                    switch (countryCode.ToUpper())
                    {
                        case "US":
                            response = Utility.Settings.USState;
                            break;
                        case "CA":
                            response = Utility.Settings.CanadaState;
                            break;
                        default:
                            response = Utility.Settings.USState;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetStates" + ex.ToString());
            }
            return response;
        }

        public static string RelativeDate(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";

            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }

        public static PieChartData GetPieChartData(DashboardData data)
        {
            PieChartData response = null;
            try
            {
                List<string> colors = new List<string>() { "#f56954", "#00a65a", "#f39c12", "#00c0ef", "#3c8dbc", "#d2d6de" };
                if (data != null && data.RouteSearch != null && data.RouteSearch.Data != null && data.RouteSearch.Data.Count > 0)
                {
                    response = new PieChartData();
                    response.labels = data.RouteSearch.Data.Select(s => s.Route).ToList<string>();
                    response.datasets = new List<Dataset>();
                    response.datasets.Add(new Dataset()
                    {
                        data = data.RouteSearch.Data.Select(s => s.Count).ToList<int>(),
                        backgroundColor = colors.Take(data.RouteSearch.Data.Count).Select(s => s).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetPieChartData" + ex.ToString());
            }
            return response;
        }
        public static BarChartData GetBarChartData(DashboardData data)
        {
            BarChartData response = null;
            try
            {
                if (data != null && data.MonthSearch != null && data.MonthSearch.Data != null && data.MonthSearch.Data.Count > 0)
                {
                    response = new BarChartData();
                    response.labels = data.MonthSearch.Data.Select(s => s.Day.ToString()).ToList<string>();
                    response.datasets = new List<BarchartDataset>();
                    response.datasets.Add(new BarchartDataset()
                    {
                        label = DateTime.UtcNow.ToString("MMMM"),
                        data = data.MonthSearch.Data.Select(s => s.Count).ToList<int>(),
                        backgroundColor = "rgba(60,141,188,0.9)",
                        borderColor = "rgba(60,141,188,0.8)",
                        pointRadius = false,
                        pointColor = "#3b8bba",
                        pointStrokeColor = "rgba(60,141,188,1)",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(60,141,188,1)",
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetBarChartData" + ex.ToString());
            }
            return response;
        }
        public static BarChartData GetLineChartData(DashboardData data)
        {
            BarChartData response = null;
            try
            {
                if (data != null && data.MonthBooking != null && data.MonthBooking.Data != null && data.MonthBooking.Data.Count > 0)
                {
                    response = new BarChartData();
                    response.labels = data.MonthBooking.Data.Select(s => s.Day.ToString()).ToList<string>();
                    response.datasets = new List<BarchartDataset>();
                    response.datasets.Add(new BarchartDataset()
                    {
                        label = DateTime.UtcNow.ToString("MMMM"),
                        data = data.MonthBooking.Data.Select(s => s.Count).ToList<int>(),
                        backgroundColor = "rgba(60,141,188,0.9)",
                        borderColor = "rgba(60,141,188,0.8)",
                        pointRadius = false,
                        pointColor = "#3b8bba",
                        pointStrokeColor = "rgba(60,141,188,1)",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(60,141,188,1)",
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetLineChartData" + ex.ToString());
            }
            return response;
        }
        public static string GetCreditCardLastDigits(string creditCardNumber)
        {
            string response = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(creditCardNumber) && creditCardNumber.Length > 4)
                {
                    creditCardNumber = creditCardNumber.Replace(" ", "");
                    response = creditCardNumber.Substring(creditCardNumber.Length - 4, 4);
                }
                else
                {
                    response = creditCardNumber;
                }
            }
            catch (Exception ex)
            {

                Utility.Logger.Error("Common.Utility.GetStates" + ex.ToString());
            }
            return response;
        }
        public static string GetCardType(int cardType)
        {
            string response = "";
            switch (cardType)
            {
                case 1:
                    response = "VI";
                    break;
                case 2:
                    response = "MC";
                    break;
                case 3:
                    response = "AX";
                    break;
                case 4:
                    response = "DC";
                    break;
                case 5:
                    response = "DS";
                    break;
                default:
                    response = "";
                    break;
            }
            return response;
        }
        public static string GetPhoneLastDigits(string phoneNumber)
        {
            string response = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length > 4)
                {
                    phoneNumber = phoneNumber.Replace(" ", "");
                    response = phoneNumber.Substring(phoneNumber.Length - 4, 4);
                }
                else
                {
                    response = phoneNumber;
                }
            }
            catch (Exception ex)
            {

                Utility.Logger.Error("Common.Utility.GetPhoneLastDigits" + ex.ToString());
            }
            return response;
        }
        public static List<UserData> GetUsers(string _email)
        {
            List<UserData> response = null;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@user", Value = !string.IsNullOrEmpty(_email) ? _email : (object)DBNull.Value });
                response = Utility.DatabaseService.ExecuteQuery<UserData>("usp_GetUsers @user", sqlParameters);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetBookingSearchList|EXCEPTION:" + ex.ToString());
            }
            return response;
        }
        public static string GetUserName(int _userId)
        {
            string response = "Not Assigned";
            try
            {
                if (Utility.Users != null && Utility.Users.Count > 0)
                {
                    var user = Utility.Users.Where(o => o.UserId.Equals(_userId)).FirstOrDefault();
                    if (user != null)
                    {
                        response = string.Format("{0} {1}", user.FirstName, user.LastName);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetUserName|EXCEPTION:" + ex.ToString());
            }
            return response;
        }
        public static BarChartData GetAffiliateSearchData(DashboardData data)
        {
            BarChartData response = null;
            try
            {
                if (data != null && data.AffiliateSearches != null && data.AffiliateSearches.Affiliates != null && data.AffiliateSearches.Affiliates.Count > 0)
                {
                    response = new BarChartData();
                    response.labels = data.AffiliateSearches.Affiliates.Select(s => s.Afffliate.ToString()).ToList<string>();
                    response.datasets = new List<BarchartDataset>();
                    response.datasets.Add(new BarchartDataset()
                    {
                        label = DateTime.UtcNow.ToString("dddd, dd"),
                        data = data.AffiliateSearches.Affiliates.Select(s => s.Count).ToList<int>(),
                        backgroundColor = "rgba(0, 100, 0)",
                        borderColor = "rgba(60,141,188,0.8)",
                        pointRadius = false,
                        pointColor = "#3b8bba",
                        pointStrokeColor = "rgba(60,141,188,1)",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = "rgba(60,141,188,1)",
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.GetAffiliateSearchData" + ex.ToString());
            }
            return response;
        }
        public static string SortInAscendingOrder(string _strVal, char _splitChar)
        {
            string res = null;
            string[] data = _strVal.Split(_splitChar);
            Array.Sort(data, (x, y) => x.CompareTo(y));
            res = string.Join("-", data);
            return res;

        }
        public static List<Currency> FetchCurrencyFromDB()
        {
            List<Currency> response = null;
            try
            {
                response = Utility.DatabaseService.ExecuteQuery<Currency>("CurrenciesGet", null);
                
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.CurrencyManager.FetchCurrencyFromDB|Exception:" + ex.ToString());
            }
            return response;
        }

        public static string GetTravellerTitle(TravellerPaxType travellerPaxType, GenderType genderType)
        {
            string response = "Mr.";
            try
            {
                switch (genderType)
                {
                    case GenderType.None:
                    case GenderType.Male:
                        if(travellerPaxType==TravellerPaxType.ADT|| travellerPaxType == TravellerPaxType.SEN)
                        {
                            response = "Mr.";
                        }
                        else
                        {
                            response = "Master.";
                        }
                        break;
                    case GenderType.Female:
                        if (travellerPaxType == TravellerPaxType.ADT || travellerPaxType == TravellerPaxType.SEN)
                        {
                            response = "Ms.";
                        }
                        else
                        {
                            response = "Master.";
                        }
                        break;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Utility.GetTravellerTitle|Exception:" + ex.ToString());
            }
            return response;
        }
    }
}
