using Common;
using Database;
using Infrastructure;
using Infrastructure.HelpingModel;
using Infrastructure.HelpingModel.BookingEntities;
using Infrastructure.HelpingModels;
using Infrastructure.HelpingModels.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SQL = Infrastructure.Entities;
namespace Business
{
    public class BookingBusiness
    {
        public static List<FlightBooking> GetBookingsBasedSearchType(int _searchParam, string _searchValue)
        {
            List<FlightBooking> bookingsInfosList = null;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@SearchParam", Value = _searchParam });
                sqlParameters.Add(new SqlParameter { ParameterName = "@SearchValue", Value = _searchValue });
                bookingsInfosList = Utility.DatabaseService.ExecuteQuery<FlightBooking>("usp_GetBookingsBasedOnParam @SearchParam, @SearchValue", sqlParameters);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetBookingsBasedSearchType | EXCEPTION:" + ex.ToString());
            }
            return bookingsInfosList;
        }
        public static List<FlightBooking> GetBookingsBasedDates(SearchCriteria search)
        {
            List<FlightBooking> bookingsInfosList = new List<FlightBooking>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                search.From = new DateTime(search.From.Year, search.From.Month, search.From.Day, 0, 0, 0);
                search.End = new DateTime(search.End.Year, search.End.Month, search.End.Day, 23, 59, 59);

                sqlParameters.Add(new SqlParameter { ParameterName = "@Disposition", Value = search.Status });
                sqlParameters.Add(new SqlParameter { ParameterName = "@SearchType", Value = (int)search.SearchType });
                sqlParameters.Add(new SqlParameter { ParameterName = "@StartDate", Value = search.From });
                sqlParameters.Add(new SqlParameter { ParameterName = "@EndDate", Value = search.End });
                sqlParameters.Add(new SqlParameter { ParameterName = "@IsOnline", Value = search.IsOnlineBookings });
                bookingsInfosList = Utility.DatabaseService.ExecuteQuery<FlightBooking>("usp_GetBookingsBasedOnDispositionV2 @Disposition, @SearchType, @StartDate, @EndDate, @IsOnline", sqlParameters);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetBookingSearchList|EXCEPTION:" + ex.ToString());
            }
            return bookingsInfosList;
        }
        public static BillingDetailsViewModel GetBillingDetailsById(int id)
        {
            BillingDetailsViewModel response = null;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@Id", Value = id });
                response = Utility.DatabaseService.ExecuteQuery<BillingDetailsViewModel>("BillingDetailsByIdGet @Id", sqlParameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetBillingDetailsById|EXCEPTION:" + ex.ToString());
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
        public static BookingDetails GetBookingDetails(int bookingId)
        {
            BookingDetails response = null;
            try
            {
                response = Procedures.GetBookingDetails(bookingId, "", Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("GetBookingDetails UNABLE  SAVE IN DATABASE|EXCEPTION:" + ex.ToString());
            }
            return response;
        }
        public static bool UpdateBookings(BookingsViewModel bookingDetail)
        {
            bool response = false;
            try
            {
                response = Procedures.UpdateBookings(bookingDetail, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.BookingInformation.UpdateBookings|Exception: " + ex.ToString());
            }
            return response;
        }
        public static bool UpdateBillingDetails(BillingDetailsViewModel billingDetail)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = Procedures.UpdateBillingDetails(billingDetail, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.BookingInformation.UpdateBillingDetails|Exception: " + ex.ToString());
            }

            return isUpdated;
        }
        public static bool UpdateFlightPriceDetails(FlightPriceDetailsViewModel priceDetail)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = Procedures.UpdateFlightPriceDetails(priceDetail, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.UpdateFlightPriceDetails|Exception: " + ex.ToString());
            }

            return isUpdated;
        }
        public static List<SQL.FlightSegments> GetFlightSegmentDetails(int bookingId)
        {
            List<SQL.FlightSegments> lstFlightSegment = null;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            try
            {
                lstFlightSegment = Procedures.GetFlightSegmentDetails(bookingId, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.GetFlightSegmentDetails|Exception: " + ex.ToString());
            }
            return lstFlightSegment;
        }
        public static bool UpdateFlightBookingSegments(List<SQL.FlightSegments> flightSegments)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = Procedures.UpdateFlightBookingSegments(flightSegments, Utility.ConnString);
            }
            catch (Exception ex)
            {
                isUpdated = false;
                Utility.Logger.Error("BookingBusiness.UpdateFlightBookingSegments|Exception: " + ex.ToString());
            }

            return isUpdated;
        }
        public static bool UpdateTraveller(TravellersViewModel traveller)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = Procedures.UpdateTraveller(traveller, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.UpdateTraveller|Exception: " + ex.ToString());
            }

            return isUpdated;
        }
        public static bool DeleteFlightTraveller(int id)
        {
            bool isUpdated = false;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@Id", Value = id });
                Utility.DatabaseService.ExecuteQuery<FlightBooking>("TravellersDelete @Id", sqlParameters);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.DeleteFlightTraveller|Exception: " + ex.ToString());
            }

            return isUpdated;
        }
        public static bool UpdateFlightBookingStatus(UpdateBookingStatusDetails updateBkStsDetail)
        {
            bool isUpdate = false;

            try
            {
                isUpdate = Procedures.UpdateFlightBookingStatus(updateBkStsDetail, Utility.ConnString);
                if (isUpdate && updateBkStsDetail.BookingStatus == BookingStatus.Cancelled)
                {
                    var res = RESTClient.CancelPNR(updateBkStsDetail.ReferenceId, ProviderType.AMADEUSSELFSERVICE).Result;
                    if (res == null || (res != null && !res.Result))
                    {
                        Utility.Logger.Error(string.Format("Cancel PNR Failed, Request:{0}", JsonConvert.SerializeObject(updateBkStsDetail)));
                    }
                    else if(res!=null && res.Result)
                    {
                        Utility.Logger.Info(string.Format("Cancel PNR success, Request:{0}", JsonConvert.SerializeObject(updateBkStsDetail)));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.BookingInformation.UpdateFlightBookingStatus | Exception: " + ex.ToString());
            }

            return isUpdate;
        }
        public static bool BookingAssign(BookingAssignModelView updateBkStsDetail)
        {
            bool isUpdate = false;
            try
            {
                isUpdate = Procedures.BookingAssign(updateBkStsDetail, Utility.ConnString);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.BookingInformation.BookingAssign | Exception: " + ex.ToString());
            }

            return isUpdate;
        }

        public static Response<BookingDetail> RetrivePNRDetails(string _referenceNo, ProviderType _providerType)
        {
            Response<BookingDetail> response = null;
            try
            {
                response = RESTClient.RetrivePNRDetails(_referenceNo, _providerType).Result;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.BookingInformation.RetrivePNRDetails | Exception: " + ex.ToString());
            }

            return response;
        }


        public static DashboardData DashboardDataGet()
        {
            DashboardData response = null;
            try
            {
                response = Procedures.GetDashboardData(Utility.ConnString);
                if (response != null)
                {
                    Utility.NewBookingCount = response.NewBooking;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("DashboardDataGet|EXCEPTION:" + ex.ToString());
            }
            return response;
        }
        public static int SaveBookingDetails(BookingDetail bookingDetails)
        {
            int tid = 0;
            try
            {
                Dictionary<string, DataTable> tables = PrepareBookingDetailsTables(bookingDetails);
                if (tables != null && tables.Count == 7)
                {
                    tid = Procedures.SaveBookingDetails(tables, Utility.ConnString);
                    if (tid == 0)
                    {
                        tid = Procedures.SaveBookingDetails(tables, Utility.ConnString);
                    }
                    if (tid == 0)
                    {
                        Utility.Logger.Info(string.Format("BOOKING UNABLE  SAVE IN DATABASE|PNR:{0}|BOOKING INFO:{1}", bookingDetails.Transaction.PNR, JsonConvert.SerializeObject(bookingDetails)));
                    }
                }
                else
                {
                    Utility.Logger.Info(string.Format("BOOKING UNABLE  SAVE IN DATABASE|PNR:{0}|BOOKING INFO:{1}", bookingDetails.Transaction.PNR, JsonConvert.SerializeObject(bookingDetails)));
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Info(string.Format("BOOKING UNABLE  SAVE IN DATABASE|PNR:{0}|BOOKING INFO:{1}", bookingDetails.Transaction.PNR, JsonConvert.SerializeObject(bookingDetails)));
                Utility.Logger.Error("BOOKING UNABLE  SAVE IN DATABASE|EXCEPTION:" + ex.ToString());
            }
            return tid;
        }
        public static Dictionary<string, DataTable> PrepareBookingDetailsTables(BookingDetail bookingDetails)
        {
            Dictionary<string, DataTable> bookingTables = null;
            try
            {
                bookingTables = new Dictionary<string, DataTable>();
                bookingTables.Add("Bookings", BookingsDataTbl(bookingDetails));
                bookingTables.Add("Flights", FlightsDataTbl(bookingDetails));
                bookingTables.Add("Segments", FlightSegmentsDataTbl(bookingDetails));
                bookingTables.Add("Travellers", TravellersDataTbl(bookingDetails));
                bookingTables.Add("PriceDetails", FlightPriceDetailsDataTbl(bookingDetails));
                bookingTables.Add("BillingDetails", BillingDetailsDataTbl(bookingDetails));
                bookingTables.Add("BookingExtras", BookingExtrasDataTbl(bookingDetails));

            }
            catch (Exception ex)
            {
                string strbookingDetails = Newtonsoft.Json.JsonConvert.SerializeObject(bookingDetails);
                Utility.Logger.Info(string.Format("BOOKING DETAILs:{0}", strbookingDetails));
                Utility.Logger.Error("EasyPro.Business.SetBookingDetails", ex.ToString());
            }
            return bookingTables;
        }

        private static DataTable BookingExtrasDataTbl(BookingDetail bookingDetails)
        {
            DataTable bookingExtras = new DataTable();
            try
            {
                bookingExtras.Columns.Add("TktTimeLimit", typeof(DateTime));
                bookingExtras.Columns.Add("IsLowcost", typeof(bool));

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.BookingsDataTbl|EXception:", ex.ToString());
            }
            return bookingExtras;
        }


        #region Private Data table Structure

        private static DataTable BookingsDataTbl(BookingDetail bookingDetails)
        {
            DataTable booking = new DataTable();
            try
            {
                booking.Columns.Add("Guid", typeof(string));
                booking.Columns.Add("PNR", typeof(string));
                booking.Columns.Add("ReferenceNumber", typeof(string));
                booking.Columns.Add("PortalId", typeof(int)).DefaultValue = bookingDetails.FlightSearch.PortalId;
                booking.Columns.Add("GDS", typeof(int));
                booking.Columns.Add("ProviderId", typeof(int));
                booking.Columns.Add("BookingType", typeof(int)).DefaultValue = (int)BookingType.Flight;
                booking.Columns.Add("BookingSourceType", typeof(int)).DefaultValue = (int)BookingSourceType.OnlineBooking;
                booking.Columns.Add("BookingStatus", typeof(int));
                booking.Columns.Add("BookingSubStatus", typeof(int)).DefaultValue = (int)BookingSubStatus.TicketAndMCOIssued;
                booking.Columns.Add("AgentId", typeof(int));
                booking.Columns.Add("AgentLead", typeof(int));
                booking.Columns.Add("UserId", typeof(int));
                booking.Columns.Add("ClientIP", typeof(string));
                booking.Columns.Add("Currency", typeof(int)).DefaultValue = (int)CurrencyType.USD;
                booking.Columns.Add("CurrencyConversion", typeof(float)).DefaultValue = 1;
                booking.Columns.Add("ImportTransactionType", typeof(int)).DefaultValue = 0;
                PopulateBooking(bookingDetails, ref booking);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.BookingsDataTbl|EXception:", ex.ToString());
            }

            return booking;
        }

        /// <summary>
        /// This method is used to Create data table for Flights table
        /// </summary>
        /// <param name="bookingDetails">BookingDetails</param>
        /// <returns>DataTable</returns>
        private static DataTable FlightsDataTbl(BookingDetail bookingDetails)
        {
            DataTable flights = new DataTable();
            try
            {
                flights.Columns.Add("OriginCode", typeof(string));
                flights.Columns.Add("DestinationCode", typeof(string));
                flights.Columns.Add("ValAirlineCode", typeof(string));
                flights.Columns.Add("TripType", typeof(int)).DefaultValue = (int)TripType.ONEWAY;
                flights.Columns.Add("DeptDate", typeof(DateTime));
                flights.Columns.Add("ReturnDate", typeof(DateTime));
                flights.Columns.Add("PaxCount", typeof(int));
                flights.Columns.Add("AdultCount", typeof(int));
                flights.Columns.Add("SeniorCount", typeof(int));
                flights.Columns.Add("ChildCount", typeof(int));
                flights.Columns.Add("InfantOnSeatCount", typeof(int));
                flights.Columns.Add("InfantLapCount", typeof(int));
                flights.Columns.Add("OutBoundFlightDuration", typeof(Int64));
                flights.Columns.Add("InBoundFlightDuration", typeof(Int64));
                flights.Columns.Add("IsDomestic", typeof(bool));
                flights.Columns.Add("FareType", typeof(string));
                flights.Columns.Add("ContractType", typeof(int)).DefaultValue = (int)ContractType.Actual;
                PopulateFlights(bookingDetails, ref flights);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.FlightsDataTbl|EXception:", ex.ToString());
            }
            return flights;
        }

        /// <summary>
        /// This method is used to Create data table for FlightSegments table
        /// </summary>
        /// <param name="bookingDetails">BookingDetails</param>
        /// <returns>DataTable</returns>
        private static DataTable FlightSegmentsDataTbl(BookingDetail bookingDetails)
        {
            DataTable flightSegments = new DataTable();
            try
            {
                flightSegments.Columns.Add("SegmentOrder", typeof(int));
                flightSegments.Columns.Add("IsReturn", typeof(bool));
                flightSegments.Columns.Add("FlightNumber", typeof(string));
                flightSegments.Columns.Add("OptAirlineCode", typeof(string));
                flightSegments.Columns.Add("MktAirlineCode", typeof(string));
                flightSegments.Columns.Add("OriginCode", typeof(string));
                flightSegments.Columns.Add("DeptDateTime", typeof(DateTime));
                flightSegments.Columns.Add("DeptTerminal", typeof(string));
                flightSegments.Columns.Add("DestinationCode", typeof(string));
                flightSegments.Columns.Add("ArrivalDateTime", typeof(DateTime));
                flightSegments.Columns.Add("ArrivalTerminal", typeof(string));
                flightSegments.Columns.Add("EquipmentDetail", typeof(string));
                flightSegments.Columns.Add("SegmentClass", typeof(string));
                flightSegments.Columns.Add("Stops", typeof(int));
                flightSegments.Columns.Add("Cabin", typeof(int));
                flightSegments.Columns.Add("CompanyFranchiseDetails", typeof(string));
                flightSegments.Columns.Add("TechnicalStoppages", typeof(string));
                flightSegments.Columns.Add("AirlineLocator", typeof(string));
                flightSegments.Columns.Add("SegmentType", typeof(string));
                PopulateFlightSegments(bookingDetails, ref flightSegments);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.FlightsDataTbl|EXception:", ex.ToString());
            }
            return flightSegments;
        }



        /// <summary>
        /// This method is used to Create data table for Travellers table
        /// </summary>
        /// <param name="bookingDetails">BookingDetails</param>
        /// <returns>DataTable</returns>
        private static DataTable TravellersDataTbl(BookingDetail bookingDetails)
        {
            DataTable travellers = new DataTable();
            try
            {
                travellers.Columns.Add("PaxOrder", typeof(int));
                travellers.Columns.Add("PaxType", typeof(int));
                travellers.Columns.Add("Title", typeof(int));
                travellers.Columns.Add("FirstName", typeof(string));
                travellers.Columns.Add("MiddleName", typeof(string));
                travellers.Columns.Add("LastName", typeof(string));
                travellers.Columns.Add("Gender", typeof(string));
                travellers.Columns.Add("DOB", typeof(DateTime));
                travellers.Columns.Add("AirlineConfirmationNo", typeof(string));
                travellers.Columns.Add("TicketNo", typeof(string));
                travellers.Columns.Add("FrequentFlyerNumber", typeof(string));
                travellers.Columns.Add("PassportNumber", typeof(string));
                travellers.Columns.Add("PassportExpireDate", typeof(DateTime));
                travellers.Columns.Add("PassportIssuedBy", typeof(string));
                travellers.Columns.Add("Email", typeof(string));
                travellers.Columns.Add("MealPreference", typeof(string));
                travellers.Columns.Add("SpecialPreference", typeof(string));
                PopulateTravellers(bookingDetails, ref travellers);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.FlightsDataTbl|EXception:", ex.ToString());
            }
            return travellers;
        }



        /// <summary>
        /// This method is used to Create data table for FlightPriceDetails table
        /// </summary>
        /// <param name="bookingDetails">BookingDetails</param>
        /// <returns>DataTable</returns>
        private static DataTable FlightPriceDetailsDataTbl(BookingDetail bookingDetails)
        {
            DataTable flightPriceDetails = new DataTable();
            try
            {
                flightPriceDetails.Columns.Add("FareBaseCode", typeof(string));
                flightPriceDetails.Columns.Add("PaxType", typeof(int));
                flightPriceDetails.Columns.Add("Currency", typeof(int));
                flightPriceDetails.Columns.Add("PaxCount", typeof(int));
                flightPriceDetails.Columns.Add("BaseFare", typeof(decimal));
                flightPriceDetails.Columns.Add("Tax", typeof(decimal));
                flightPriceDetails.Columns.Add("Markup", typeof(decimal));
                flightPriceDetails.Columns.Add("SupplierFee", typeof(decimal));
                flightPriceDetails.Columns.Add("Discount", typeof(decimal));
                flightPriceDetails.Columns.Add("IsSellInsurance", typeof(bool));
                flightPriceDetails.Columns.Add("InsuranceAmount", typeof(decimal));
                flightPriceDetails.Columns.Add("TotalAmount", typeof(decimal));
                flightPriceDetails.Columns.Add("IsSellBaggageInsurance", typeof(bool));
                flightPriceDetails.Columns.Add("BaggageInsuranceAmount", typeof(decimal));
                flightPriceDetails.Columns.Add("IsExtendedCancellation", typeof(bool));
                flightPriceDetails.Columns.Add("ExtendedCancellationAmount", typeof(decimal));
                flightPriceDetails.Columns.Add("BookingFee", typeof(decimal));
                PopulateFlightPriceDetails(bookingDetails, ref flightPriceDetails);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.FlightPriceDetails|EXception:", ex.ToString());
            }
            return flightPriceDetails;
        }


        /// <summary>
        /// This method is used to Create data table for BillingDetails table
        /// </summary>
        /// <param name="bookingDetails">BookingDetails</param>
        /// <returns>DataTable</returns>
        private static DataTable BillingDetailsDataTbl(BookingDetail bookingDetails)
        {
            DataTable billingDetails = new DataTable();
            try
            {
                billingDetails.Columns.Add("CCHolderName", typeof(string));
                billingDetails.Columns.Add("CardNumber", typeof(string));
                billingDetails.Columns.Add("CVVNumber", typeof(string));
                billingDetails.Columns.Add("ExpiryYear", typeof(int));
                billingDetails.Columns.Add("ExpiryMonth", typeof(int));
                billingDetails.Columns.Add("CardType", typeof(int));
                billingDetails.Columns.Add("Email", typeof(string));
                billingDetails.Columns.Add("Country", typeof(string));
                billingDetails.Columns.Add("State", typeof(string));
                billingDetails.Columns.Add("ZipCode", typeof(string));
                billingDetails.Columns.Add("AddressLine1", typeof(string));
                billingDetails.Columns.Add("AddressLine2", typeof(string));
                billingDetails.Columns.Add("AddressLine3", typeof(string));
                billingDetails.Columns.Add("City", typeof(string));
                billingDetails.Columns.Add("BillingPhone", typeof(string));
                billingDetails.Columns.Add("ContactPhone", typeof(string));
                billingDetails.Columns.Add("IsPrimaryCard", typeof(string));
                billingDetails.Columns.Add("AreaCode", typeof(string));
                billingDetails.Columns.Add("CountryCode", typeof(string));
                PopulateBillingDetails(bookingDetails, ref billingDetails);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.BillingDetails|EXception:", ex.ToString());
            }
            return billingDetails;
        }


        public static bool SetAsDefaultCoupon(int id)
        {
            bool response = false;
            try
            {
                CouponDefaultRES res = new CouponDefaultRES();
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@Id", Value = id });
                res = Utility.DatabaseService.ExecuteQuery<CouponDefaultRES>("CouponsSetDefault @Id", sqlParameters).FirstOrDefault();
                if (res != null)
                {
                    response = res.Result;
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingBusiness.SetAsDefaultCoupon|EXCEPTION:" + ex.ToString());
            }
            return response;
        }

        #endregion



        #region Private to populate Data Table
        /// <summary>
        /// This method is used to set booking values for MFSPaymentDetails table
        /// </summary>
        /// <param name="context">BookingDetails</param>
        /// <param name="booking">DataTable</param>
        private static void PopulateBooking(BookingDetail bookingDetails, ref DataTable booking)
        {
            try
            {
                DataRow dataRow;
                dataRow = booking.NewRow();
                dataRow["Guid"] = Guid.NewGuid().ToString("N");
                dataRow["PNR"] = bookingDetails.Transaction.PNR;
                dataRow["ReferenceNumber"] = bookingDetails.Transaction.ReferenceNumber;
                dataRow["PortalId"] = bookingDetails.Transaction.PortalId;
                dataRow["GDS"] = bookingDetails.Transaction.GDS;
                dataRow["ProviderId"] = bookingDetails.Transaction.ProviderId;
                dataRow["BookingType"] = bookingDetails.Transaction.BookingType;
                dataRow["BookingSourceType"] = bookingDetails.Transaction.BookingSourceType;
                dataRow["BookingStatus"] = bookingDetails.Transaction.BookingStatus;
                dataRow["BookingSubStatus"] = bookingDetails.Transaction.BookingSubStatus;
                dataRow["AgentId"] = bookingDetails.Transaction.AgentId;
                dataRow["AgentLead"] = bookingDetails.Transaction.AgentLead;
                dataRow["UserId"] = bookingDetails.Transaction.UserId;
                dataRow["ClientIP"] = bookingDetails.FlightSearch.IP;
                dataRow["Currency"] = bookingDetails.Currency != CurrencyType.None ? (int)bookingDetails.Currency : (int)CurrencyType.USD;
                dataRow["CurrencyConversion"] = bookingDetails.CurrencyConversion > 0 ? bookingDetails.CurrencyConversion : 1;
                dataRow["ImportTransactionType"] = 0;
                booking.Rows.Add(dataRow);
                booking.AcceptChanges();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateTransaction|EXception:", ex.ToString());
            }
        }
        private static void PopulateFlights(BookingDetail bookingDetails, ref DataTable flights)
        {
            try
            {
                DataRow dataRow;
                dataRow = flights.NewRow();
                dataRow["OriginCode"] = bookingDetails.Contract.Origin;
                dataRow["DestinationCode"] = bookingDetails.Contract.Destination;
                dataRow["ValAirlineCode"] = bookingDetails.Contract.ValidatingCarrier.Code;
                dataRow["TripType"] = (int)bookingDetails.Contract.TripType;
                dataRow["DeptDate"] = bookingDetails.Contract.DepartureDate;
                dataRow["ReturnDate"] = (bookingDetails.Contract.TripType == TripType.ONEWAY ? DateTime.Now.Date : bookingDetails.Contract.ArrivalDate);
                dataRow["PaxCount"] = bookingDetails.Contract.GetTotalPax();
                dataRow["AdultCount"] = bookingDetails.Contract.Adult;
                dataRow["SeniorCount"] = bookingDetails.Contract.Senior;
                dataRow["ChildCount"] = bookingDetails.Contract.Child;
                dataRow["InfantOnSeatCount"] = bookingDetails.Contract.InfantOnSeat;
                dataRow["InfantLapCount"] = bookingDetails.Contract.InfantOnLap;
                dataRow["OutBoundFlightDuration"] = bookingDetails.Contract.TotalOutBoundFlightDuration != TimeSpan.MinValue ? (Int64)bookingDetails.Contract.TotalOutBoundFlightDuration.TotalMinutes : 0;
                dataRow["InBoundFlightDuration"] = bookingDetails.Contract.TotalInBoundFlightDuration != TimeSpan.MinValue ? (Int64)bookingDetails.Contract.TotalInBoundFlightDuration.TotalMinutes : 0; dataRow["IsDomestic"] = false;
                dataRow["FareType"] = bookingDetails.Contract.FareType;
                dataRow["ContractType"] = (int)bookingDetails.Contract.ContractType;
                flights.Rows.Add(dataRow);
                flights.AcceptChanges();

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateFlights|EXception:", ex.ToString());
            }
        }
        private static void PopulateFlightSegments(BookingDetail bookingDetails, ref DataTable flightSegments)
        {

            try
            {
                DataRow dataRow;
                int i = 0;
                foreach (Segments item in bookingDetails.Contract.TripDetails.OutBoundSegment)
                {
                    dataRow = flightSegments.NewRow();
                    dataRow["SegmentOrder"] = i;
                    dataRow["IsReturn"] = false;
                    dataRow["FlightNumber"] = item.FlightNumber;
                    dataRow["OptAirlineCode"] = item.OperatingCarrier != null ? item.OperatingCarrier.Code : item.MarketingCarrier.Code;
                    dataRow["MktAirlineCode"] = item.MarketingCarrier.Code;
                    dataRow["OriginCode"] = item.Origin;
                    dataRow["DeptDateTime"] = new DateTime(item.Departure.Year, item.Departure.Month, item.Departure.Day).Add(item.DepartureTime);
                    dataRow["DeptTerminal"] = item.OutTerminal;
                    dataRow["DestinationCode"] = item.Destination;
                    dataRow["ArrivalDateTime"] = new DateTime(item.Arrival.Year, item.Arrival.Month, item.Arrival.Day).Add(item.ArrivalTime);
                    dataRow["ArrivalTerminal"] = item.InTerminal;
                    dataRow["EquipmentDetail"] = item.EquipmentType;
                    dataRow["SegmentClass"] = item.Class;
                    dataRow["Stops"] = item.NoOfStops;
                    dataRow["Cabin"] = (int)item.CabinType;
                    dataRow["CompanyFranchiseDetails"] = item.CompanyFranchiseDetails;
                    dataRow["TechnicalStoppages"] = "";
                    dataRow["AirlineLocator"] = item.AirlineLocator;
                    dataRow["SegmentType"] = string.IsNullOrEmpty(item.SegmentStatus) == true ? "" : item.SegmentStatus;
                    flightSegments.Rows.Add(dataRow);
                    flightSegments.AcceptChanges();
                    i++;
                }
                if (bookingDetails.Contract.TripType == TripType.ROUNDTRIP && bookingDetails.Contract.TripDetails.InBoundSegment != null && bookingDetails.Contract.TripDetails.InBoundSegment.Count > 0)
                {
                    foreach (Segments item in bookingDetails.Contract.TripDetails.InBoundSegment)
                    {
                        dataRow = flightSegments.NewRow();
                        dataRow["SegmentOrder"] = i;
                        dataRow["IsReturn"] = true;
                        dataRow["FlightNumber"] = item.FlightNumber;
                        dataRow["OptAirlineCode"] = item.OperatingCarrier != null ? item.OperatingCarrier.Code : item.MarketingCarrier.Code;
                        dataRow["MktAirlineCode"] = item.MarketingCarrier.Code;
                        dataRow["OriginCode"] = item.Origin;
                        dataRow["DeptDateTime"] = new DateTime(item.Departure.Year, item.Departure.Month, item.Departure.Day).Add(item.DepartureTime);
                        dataRow["DeptTerminal"] = item.OutTerminal;
                        dataRow["DestinationCode"] = item.Destination;
                        dataRow["ArrivalDateTime"] = new DateTime(item.Arrival.Year, item.Arrival.Month, item.Arrival.Day).Add(item.ArrivalTime);
                        dataRow["ArrivalTerminal"] = item.InTerminal;
                        dataRow["EquipmentDetail"] = item.EquipmentType;
                        dataRow["SegmentClass"] = item.Class;
                        dataRow["Stops"] = item.NoOfStops;
                        dataRow["Cabin"] = (int)item.CabinType;
                        dataRow["CompanyFranchiseDetails"] = item.CompanyFranchiseDetails;
                        dataRow["TechnicalStoppages"] = "";
                        dataRow["AirlineLocator"] = item.AirlineLocator;
                        dataRow["SegmentType"] = "";
                        flightSegments.Rows.Add(dataRow);
                        flightSegments.AcceptChanges();
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateFlightSegments|EXception:", ex.ToString());
            }
        }
        private static void PopulateTravellers(BookingDetail bookingDetails, ref DataTable travellers)
        {
            try
            {
                if (bookingDetails.Travellers != null && bookingDetails.Travellers.Count > 0)
                {
                    DataRow dataRow;
                    int i = 0;
                    foreach (Traveller item in bookingDetails.Travellers)
                    {
                        dataRow = travellers.NewRow();
                        dataRow["PaxOrder"] = i;
                        dataRow["PaxType"] = (int)item.PaxType;
                        dataRow["Title"] = item.Title;
                        dataRow["FirstName"] = item.FirstName;
                        dataRow["MiddleName"] = item.MiddleName;
                        dataRow["LastName"] = item.LastName;
                        dataRow["Gender"] = item.Gender;
                        dataRow["DOB"] = item.DOBYear == 0 && item.DOBMonth == 0 && item.DOBDay == 0 ? (object)DBNull.Value : new DateTime(item.DOBYear ?? 0, item.DOBMonth, item.DOBDay ?? 0);
                        dataRow["AirlineConfirmationNo"] = null;
                        dataRow["TicketNo"] = null;
                        dataRow["FrequentFlyerNumber"] = null;
                        dataRow["PassportNumber"] = item.PassportNumber;
                        if (item.PassportExpiryDate == null || (item.PassportExpiryDate != null && item.PassportExpiryDate == DateTime.MinValue))
                        {
                            dataRow["PassportExpireDate"] = DBNull.Value;
                        }
                        else
                        {
                            dataRow["PassportExpireDate"] = item.PassportExpiryDate;
                        }

                        dataRow["PassportIssuedBy"] = item.PassportIssuingCountry;
                        dataRow["Email"] = null;
                        dataRow["MealPreference"] = null;
                        dataRow["SpecialPreference"] = null;
                        travellers.Rows.Add(dataRow);
                        travellers.AcceptChanges();
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateTravellers|EXception:", ex.ToString());
            }
        }
        private static void PopulateBillingDetails(BookingDetail bookingDetails, ref DataTable tblBillingDetails)
        {
            try
            {
                if (bookingDetails.BillingDetails != null)
                {
                    DataRow dataRow;
                    BillingDetail item = bookingDetails.BillingDetails;
                    dataRow = tblBillingDetails.NewRow();
                    dataRow["CCHolderName"] = item.CCHolderName;
                    dataRow["CardNumber"] = !string.IsNullOrEmpty(item.CardNumber) ? item.CardNumber.Trim() : string.Empty;
                    dataRow["CVVNumber"] = item.CVVNumber;
                    dataRow["ExpiryYear"] = item.ExpiryYear;
                    dataRow["ExpiryMonth"] = item.ExpiryMonth;
                    dataRow["CardType"] = item.CardType;
                    dataRow["Email"] = item.Email;
                    dataRow["Country"] = item.Country;
                    dataRow["State"] = (item.Country.Equals("US", StringComparison.OrdinalIgnoreCase) || item.Country.Equals("CA", StringComparison.OrdinalIgnoreCase)) ? item.State : item.StateName;
                    dataRow["ZipCode"] = item.ZipCode;
                    dataRow["AddressLine1"] = item.AddressLine1;
                    dataRow["AddressLine2"] = item.AddressLine2;
                    dataRow["AddressLine3"] = item.AddressLine3;
                    dataRow["City"] = item.City;
                    dataRow["AreaCode"] = item.AreaCode;
                    dataRow["CountryCode"] = item.CountryCode;
                    dataRow["BillingPhone"] = item.BillingPhone;
                    dataRow["ContactPhone"] = item.ContactPhone;
                    dataRow["IsPrimaryCard"] = item.IsPrimaryCard;
                    tblBillingDetails.Rows.Add(dataRow);
                    tblBillingDetails.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateBillingDetails|EXception:", ex.ToString());
            }
        }
        private static void PopulateFlightPriceDetails(BookingDetail bookingDetails, ref DataTable flightPriceDetails)
        {
            try
            {
                List<FareDetails> lstFareDetail = new List<FareDetails>();

                if (bookingDetails.Contract.AdultFare != null && bookingDetails.Contract.Adult > 0)
                {
                    lstFareDetail.Add(bookingDetails.Contract.AdultFare);
                }
                if (bookingDetails.Contract.SeniorFare != null && bookingDetails.Contract.Senior > 0)
                {
                    lstFareDetail.Add(bookingDetails.Contract.SeniorFare);
                }
                if (bookingDetails.Contract.ChildFare != null && bookingDetails.Contract.Child > 0)
                {
                    lstFareDetail.Add(bookingDetails.Contract.ChildFare);
                }
                if (bookingDetails.Contract.InfantOnSeatFare != null && bookingDetails.Contract.InfantOnSeat > 0)
                {
                    lstFareDetail.Add(bookingDetails.Contract.InfantOnSeatFare);
                }
                if (bookingDetails.Contract.InfantOnLapFare != null && bookingDetails.Contract.InfantOnLap > 0)
                {
                    lstFareDetail.Add(bookingDetails.Contract.InfantOnLapFare);
                }

                if (lstFareDetail != null && lstFareDetail.Count > 0)
                {
                    bool isBaggageInsurance = false;
                    decimal baggageAmount = 0.0M;
                    if (bookingDetails.BagInsuranc != null && bookingDetails.BagInsuranc.BagInsuranceType != BagInsuranceType.NONE)
                    {
                        isBaggageInsurance = true;
                        baggageAmount = bookingDetails.BagInsuranc.PPaxPrice;
                    }
                    bool isTravelInsurance = false;
                    decimal travelAmount = 0.0M;
                    if (bookingDetails.TravelerInsurance != null && bookingDetails.TravelerInsurance.IsTravelProtected)
                    {
                        isTravelInsurance = true;
                        travelAmount = bookingDetails.TravelerInsurance.PPaxPrice;
                    }

                    bool isExtendedCancellation = false;
                    decimal ExtendedCancellationAmount = 0.0M;
                    if (bookingDetails.ExtendedCancellation != null && bookingDetails.ExtendedCancellation.IsExtendedCancellation)
                    {
                        isExtendedCancellation = true;
                        ExtendedCancellationAmount = bookingDetails.ExtendedCancellation.PPaxPrice;
                    }

                    DataRow dataRow;
                    foreach (FareDetails item in lstFareDetail)
                    {
                        dataRow = flightPriceDetails.NewRow();
                        dataRow["FareBaseCode"] = item.FareBaseCode;
                        dataRow["PaxType"] = (int)item.PaxType;
                        dataRow["Currency"] = (int)item.CurrencyType;
                        dataRow["PaxCount"] = item.PaxCount;
                        dataRow["BaseFare"] = item.BaseFare;
                        dataRow["Tax"] = item.Tax;
                        dataRow["Markup"] = item.Markup;
                        dataRow["SupplierFee"] = item.SupplierFee;
                        dataRow["Discount"] = item.Discount;
                        item.InsuranceAmount = Convert.ToSingle(travelAmount);
                        item.IsSellInsurance = isTravelInsurance;
                        dataRow["IsSellInsurance"] = isTravelInsurance;
                        dataRow["InsuranceAmount"] = travelAmount;
                        dataRow["TotalAmount"] = item.TotalFareV2;
                        item.BaggageInsuranceAmount = Convert.ToSingle(baggageAmount);
                        item.IsSellBaggageInsurance = isBaggageInsurance;
                        dataRow["IsSellBaggageInsurance"] = isBaggageInsurance;
                        dataRow["BaggageInsuranceAmount"] = baggageAmount;
                        item.IsExtendedCancellation = isExtendedCancellation;
                        dataRow["IsExtendedCancellation"] = isExtendedCancellation;
                        dataRow["ExtendedCancellationAmount"] = ExtendedCancellationAmount;
                        dataRow["BookingFee"] = item.BookingFee;
                        flightPriceDetails.Rows.Add(dataRow);
                        flightPriceDetails.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.PopulateFlightPriceDetails|EXception:", ex.ToString());
            }
        }

        #endregion
    }
    public class CouponDefaultRES
    {
        public bool Result { get; set; }
    }
}
