using Infrastructure;
using Infrastructure.HelpingModel.BookingEntities;
using Infrastructure.HelpingModels;
using Infrastructure.HelpingModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SQL = Infrastructure.Entities;

namespace Database
{
    public class Procedures
    {
        public static BookingDetails GetBookingDetails(int bookingId, string guid, string connectionString)
        {
            BookingDetails response = null;
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter sqlDataAdapter;
                    DataSet ds = new DataSet();
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "usp_GetBookingDetails";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@bookingId", bookingId);
                        command.Parameters.AddWithValue("@guid", guid);
                        sqlDataAdapter = new SqlDataAdapter(command);
                        sqlDataAdapter.Fill(ds);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            response = new BookingDetails();

                            for (int i = 0; i <= ds.Tables.Count - 1; i++)
                            {
                                if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                {
                                    switch (i)
                                    {
                                        case 0://Bookings
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.Transaction = new Transactions();
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.Transaction.Id = Convert.ToInt32(row["Id"]);
                                                    response.Transaction.PNR = row["PNR"] == DBNull.Value ? string.Empty : Convert.ToString(row["PNR"]);
                                                    response.Transaction.ReferenceNumber = row["ReferenceNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["ReferenceNumber"]);
                                                    response.Transaction.PortalId = Convert.ToInt32(row["PortalId"]);
                                                    response.Transaction.GDS = Convert.ToInt32(row["GDS"]);
                                                    response.Transaction.ProviderId = Convert.ToInt32(row["ProviderId"]);
                                                    response.Transaction.BookingType = Convert.ToInt32(row["BookingType"]);
                                                    response.Transaction.BookingSourceType = Convert.ToInt32(row["BookingSourceType"]);
                                                    response.Transaction.BookingStatus = Convert.ToInt32(row["BookingStatus"]);
                                                    response.Transaction.BookingSubStatus = Convert.ToInt32(row["BookingSubStatus"]);
                                                    response.Transaction.AgentId = Convert.ToInt32(row["AgentId"]);
                                                    response.Transaction.AgentLead = Convert.ToInt32(row["AgentLead"]);
                                                    response.Transaction.UserId = Convert.ToInt32(row["UserId"]);
                                                    response.Transaction.BookedOn = DateTime.SpecifyKind(Convert.ToDateTime(row["Created"]), DateTimeKind.Utc);
                                                    response.Transaction.ClientIP = row["ClientIP"] == DBNull.Value ? string.Empty : Convert.ToString(row["ClientIP"]);
                                                    response.Transaction.AirlinePNR = row["AirlinePNR"] == DBNull.Value ? string.Empty : Convert.ToString(row["AirlinePNR"]);
                                                    response.Transaction.CurrencyCode = row["CurrencyCode"] == DBNull.Value ? "USD" : Convert.ToString(row["CurrencyCode"]);
                                                    response.Transaction.CurrencyPrice = row["CurrencyConversion"] == DBNull.Value ? 1 : Convert.ToDecimal(row["CurrencyConversion"]);
                                                    response.Transaction.IsFailedBooking = row["IsFailedBooking"] == DBNull.Value ? false : Convert.ToBoolean(row["IsFailedBooking"]);
                                                    response.Transaction.BookingFailedErrorMessage = row["BookingFailedErrorMessage"] == DBNull.Value ? string.Empty : Convert.ToString(row["BookingFailedErrorMessage"]);
                                                    break;
                                                }
                                            }
                                            break;
                                        case 1: //Flights
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.Flight = new Flights();
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.Flight.TransactionId = Convert.ToInt32(row["BookingId"]);
                                                    response.Flight.Origin = row["OriginCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["OriginCode"]);
                                                    response.Flight.Destination = row["DestinationCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["DestinationCode"]);
                                                    response.Flight.Airline = row["ValAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["ValAirlineCode"]);
                                                    response.Flight.TripType = Convert.ToInt32(row["TripType"]);
                                                    response.Flight.DeptDate = Convert.ToDateTime(row["DeptDate"]);
                                                    response.Flight.ReturnDate = row["ReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ReturnDate"]);
                                                    response.Flight.TotalPaxCount = Convert.ToInt32(row["PaxCount"]);
                                                    response.Flight.AdultCount = Convert.ToInt32(row["AdultCount"]);
                                                    response.Flight.SeniorCount = Convert.ToInt32(row["SeniorCount"]);
                                                    response.Flight.ChildCount = Convert.ToInt32(row["ChildCount"]);
                                                    response.Flight.InfantCount = Convert.ToInt32(row["InfantOnSeatCount"]);
                                                    response.Flight.InfantLapCount = Convert.ToInt32(row["InfantLapCount"]);
                                                    response.Flight.OutBoundFlightDuration = Convert.ToInt64(row["OutBoundFlightDuration"]);
                                                    response.Flight.InBoundFlightDuration = Convert.ToInt64(row["InBoundFlightDuration"]);
                                                    response.Flight.IsDomestic = Convert.ToBoolean(row["IsDomestic"]);
                                                    response.Flight.FareType = row["FareType"] == DBNull.Value ? "" : Convert.ToString(row["FareType"]);
                                                    response.Flight.ContractType = Convert.ToInt32(row["ContractType"]);
                                                    break;
                                                }
                                            }
                                            break;
                                        case 2: //FlightSegments
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.Segments = new List<FlightSegments>();
                                                FlightSegments segment = null;
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    segment = new FlightSegments();
                                                    segment.TransactionId = Convert.ToInt32(row["BookingId"]);
                                                    segment.SegmentOrder = Convert.ToInt32(row["SegmentOrder"]);
                                                    segment.IsReturn = Convert.ToBoolean(row["IsReturn"]);
                                                    segment.FlightNumber = row["FlightNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["FlightNumber"]);
                                                    segment.MarketingCode = row["MktAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["MktAirlineCode"]);
                                                    segment.OperatingCode = row["OptAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["OptAirlineCode"]);
                                                    segment.Origin = row["OriginCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["OriginCode"]);
                                                    segment.DeptDateTime = Convert.ToDateTime(row["DeptDateTime"]);
                                                    segment.DeptTerminal = row["DeptTerminal"] == DBNull.Value ? string.Empty : Convert.ToString(row["DeptTerminal"]);
                                                    segment.Destination = row["DestinationCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["DestinationCode"]);
                                                    segment.ArrivalDateTime = Convert.ToDateTime(row["ArrivalDateTime"]);
                                                    segment.DeptTerminal = row["ArrivalTerminal"] == DBNull.Value ? string.Empty : Convert.ToString(row["ArrivalTerminal"]);
                                                    segment.EquipmentDetail = row["EquipmentDetail"] == DBNull.Value ? string.Empty : Convert.ToString(row["EquipmentDetail"]);
                                                    segment.SegmentClass = row["SegmentClass"] == DBNull.Value ? string.Empty : Convert.ToString(row["SegmentClass"]);
                                                    segment.Stops = Convert.ToInt32(row["Stops"]);
                                                    segment.Cabin = Convert.ToInt32(row["Cabin"]);
                                                    segment.CompanyFranchiseDetails = row["CompanyFranchiseDetails"] == DBNull.Value ? string.Empty : Convert.ToString(row["CompanyFranchiseDetails"]);
                                                    segment.TechnicalStoppages = row["TechnicalStoppages"] == DBNull.Value ? string.Empty : Convert.ToString(row["TechnicalStoppages"]);
                                                    segment.AirlineLocator = row["AirlineLocator"] == DBNull.Value ? string.Empty : Convert.ToString(row["AirlineLocator"]);
                                                    response.Segments.Add(segment);
                                                }
                                            }
                                            break;
                                        case 3: //Flight Price Details
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.PriceDetail = new List<PriceDetails>();
                                                PriceDetails priceDetail = null;
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    priceDetail = new PriceDetails();
                                                    priceDetail.Id = Convert.ToInt32(row["Id"]);
                                                    priceDetail.TransactionId = Convert.ToInt32(row["BookingId"]);
                                                    priceDetail.FareBaseCode = row["FareBaseCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["FareBaseCode"]);
                                                    priceDetail.PaxType = Convert.ToInt32(row["PaxType"]);
                                                    priceDetail.Currency = Convert.ToInt32(row["Currency"]);
                                                    priceDetail.PaxCount = Convert.ToInt32(row["PaxCount"]);
                                                    priceDetail.BaseFare = Convert.ToDecimal(row["BaseFare"]);
                                                    priceDetail.Tax = Convert.ToDecimal(row["Tax"]);
                                                    priceDetail.Markup = Convert.ToDecimal(row["Markup"]);
                                                    priceDetail.SupplierFee = Convert.ToDecimal(row["SupplierFee"]);
                                                    priceDetail.Discount = Convert.ToDecimal(row["Discount"]);
                                                    priceDetail.IsSellInsurance = Convert.ToBoolean(row["IsSellInsurance"]);
                                                    priceDetail.InsuranceAmount = Convert.ToDecimal(row["InsuranceAmount"]);
                                                    priceDetail.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                                                    priceDetail.IsSellBaggageInsurance = Convert.ToBoolean(row["IsSellBaggageInsurance"]);
                                                    priceDetail.BaggageInsuranceAmount = Convert.ToDecimal(row["BaggageInsuranceAmount"]);
                                                    priceDetail.IsExtendedCancellation = Convert.ToBoolean(row["IsExtendedCancellation"]);
                                                    priceDetail.ExtendedCancellationAmount = Convert.ToDecimal(row["ExtendedCancellationAmount"]);
                                                    priceDetail.BookingFee = Convert.ToDecimal(row["BookingFee"]);
                                                    response.PriceDetail.Add(priceDetail);
                                                }
                                            }
                                            break;
                                        case 4: //BillingDetails
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.BillingDetails = new BillingDetails();
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.BillingDetails.Id = Convert.ToInt32(row["Id"]);
                                                    response.BillingDetails.TransactionId = Convert.ToInt32(row["BookingId"]);
                                                    response.BillingDetails.CCHolderName = row["CCHolderName"] == DBNull.Value ? string.Empty : Convert.ToString(row["CCHolderName"]);
                                                    response.BillingDetails.CardNumber = row["CardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["CardNumber"]);
                                                    response.BillingDetails.CVVNumber = row["CVVNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["CVVNumber"]);


                                                    response.BillingDetails.ExpiryYear = Convert.ToInt32(row["ExpiryYear"]);
                                                    response.BillingDetails.ExpiryMonth = Convert.ToInt32(row["ExpiryMonth"]);
                                                    response.BillingDetails.CardType = Convert.ToInt32(row["CardType"]);
                                                    response.BillingDetails.Email = row["Email"] == DBNull.Value ? string.Empty : Convert.ToString(row["Email"]);
                                                    response.BillingDetails.Country = row["Country"] == DBNull.Value ? string.Empty : Convert.ToString(row["Country"]);
                                                    response.BillingDetails.State = row["State"] == DBNull.Value ? string.Empty : Convert.ToString(row["State"]);
                                                    response.BillingDetails.ZipCode = row["ZipCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["ZipCode"]);
                                                    response.BillingDetails.AddressLine1 = row["AddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(row["AddressLine1"]);
                                                    response.BillingDetails.AddressLine2 = row["AddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(row["AddressLine2"]);
                                                    response.BillingDetails.AddressLine3 = row["AddressLine3"] == DBNull.Value ? string.Empty : Convert.ToString(row["AddressLine3"]);
                                                    response.BillingDetails.City = row["City"] == DBNull.Value ? string.Empty : Convert.ToString(row["City"]);
                                                    response.BillingDetails.BillingPhone = row["BillingPhone"] == DBNull.Value ? string.Empty : Convert.ToString(row["BillingPhone"]);
                                                    response.BillingDetails.ContactPhone = row["ContactPhone"] == DBNull.Value ? string.Empty : Convert.ToString(row["ContactPhone"]);
                                                    response.BillingDetails.IsPrimaryCard = Convert.ToBoolean(row["IsPrimaryCard"]);
                                                    break;
                                                }
                                            }
                                            break;
                                        case 5: //Traveller Details
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.Travellers = new List<Travellers>();
                                                Travellers traveller = null;
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    traveller = new Travellers();
                                                    traveller.Id = Convert.ToInt32(row["Id"]);
                                                    traveller.TransactionId = Convert.ToInt32(row["BookingId"]);
                                                    traveller.PaxOrderId = Convert.ToInt32(row["PaxOrder"]);
                                                    traveller.PaxType = Convert.ToInt32(row["PaxType"]);
                                                    traveller.Title = Convert.ToInt32(row["Title"]);
                                                    traveller.FirstName = row["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(row["FirstName"]);
                                                    traveller.MiddleName = row["MiddleName"] == DBNull.Value ? string.Empty : Convert.ToString(row["MiddleName"]);
                                                    traveller.LastName = row["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastName"]);
                                                    traveller.Gender = Convert.ToInt32(row["Gender"]);
                                                    traveller.DOB = row["DOB"] == DBNull.Value ? (DateTime?)null:Convert.ToDateTime(row["DOB"]);
                                                    traveller.TicketsNo = row["TicketNo"] == DBNull.Value ? string.Empty : Convert.ToString(row["TicketNo"]);
                                                    response.Travellers.Add(traveller);
                                                }
                                            }
                                            break;

                                        case 6: //Baggage Protuction
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.BaggageInsurances = new BaggageInsurances();

                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.BaggageInsurances.BookingId = Convert.ToInt32(row["BookingId"]);
                                                    response.BaggageInsurances.ServiceNumber = row["ServiceNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["ServiceNumber"]);
                                                    response.BaggageInsurances.ErrorCode = row["ErrorCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["ErrorCode"]);
                                                    response.BaggageInsurances.ErrorMessage = row["ErrorMessage"] == DBNull.Value ? string.Empty : Convert.ToString(row["ErrorMessage"]);
                                                    response.BaggageInsurances.ProductId = Convert.ToInt32(row["ProductId"]);
                                                    response.BaggageInsurances.ProductName = row["ProductName"] == DBNull.Value ? string.Empty : Convert.ToString(row["ProductName"]);
                                                    response.BaggageInsurances.Status = Convert.ToBoolean(row["Status"]);
                                                    response.BaggageInsurances.StatusCode = row["StatusCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["StatusCode"]);
                                                    response.BaggageInsurances.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);


                                                }
                                            }
                                            break;
                                        case 7: //Travel Protuction
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.TravelInsurance = new TravelInsurance();
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.TravelInsurance.BookingId = Convert.ToInt32(row["BookingId"]);
                                                    response.TravelInsurance.PolicyNumber = row["PolicyNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["PolicyNumber"]);
                                                    response.TravelInsurance.RefNumber = row["RefNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["RefNumber"]);
                                                    response.TravelInsurance.GroupNumber = row["GroupNumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["GroupNumber"]);
                                                    response.TravelInsurance.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);
                                                    response.TravelInsurance.Warnings = row["Warnings"] == DBNull.Value ? string.Empty : Convert.ToString(row["Warnings"]);
                                                }
                                            }
                                            break;
                                        case 8: //Coupon Details
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.CouponDetails = new Coupon();
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.CouponDetails.CouponCode = row["CouponCode"] == DBNull.Value ? string.Empty : Convert.ToString(row["CouponCode"]);
                                                    response.CouponDetails.CouponMessage = row["CouponMessage"] == DBNull.Value ? string.Empty : Convert.ToString(row["CouponMessage"]);
                                                    response.CouponDetails.Status = row["Status"] == DBNull.Value ? false : Convert.ToBoolean(row["Status"]);
                                                    response.CouponDetails.TotalAmount = row["TotalAmount"] == DBNull.Value ? 0 : Convert.ToSingle(row["TotalAmount"]);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public static bool UpdateBookings(BookingsViewModel bookingDetail, string connectionString)
        {
            bool response = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "[dbo].[BookingsUpdate]";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@Id", bookingDetail.Id);
                        command.Parameters.AddWithValue("@PNR", bookingDetail.PNR);
                        command.Parameters.AddWithValue("@PortalId", bookingDetail.PortalId);
                        command.Parameters.AddWithValue("@ProviderId", bookingDetail.ProviderId);
                        command.Parameters.AddWithValue("@BookingSourceType", bookingDetail.BookingSourceType);
                        command.Parameters.AddWithValue("@AirlinePNR", bookingDetail.AirlinePNR);
                        command.ExecuteNonQuery();
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public static bool UpdateBillingDetails(BillingDetailsViewModel billingDetail, string connectionString)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "dbo.BillingDetailsUpdate";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;

                        command.Parameters.AddWithValue("@Id", billingDetail.Id);
                        command.Parameters.AddWithValue("@BookingId", billingDetail.BookingId);
                        command.Parameters.AddWithValue("@CCHolderName", billingDetail.CCHolderName);
                        command.Parameters.AddWithValue("@CardNumber", billingDetail.CardNumber);
                        command.Parameters.AddWithValue("@CVVNumber", billingDetail.CVVNumber);
                        command.Parameters.AddWithValue("@ExpiryYear", billingDetail.ExpiryYear);
                        command.Parameters.AddWithValue("@ExpiryMonth", billingDetail.ExpiryMonth);
                        command.Parameters.AddWithValue("@CardType", billingDetail.CardType);
                        command.Parameters.AddWithValue("@Email", billingDetail.Email);
                        command.Parameters.AddWithValue("@Country", billingDetail.Country);
                        command.Parameters.AddWithValue("@State", billingDetail.State);
                        command.Parameters.AddWithValue("@ZipCode", billingDetail.ZipCode);
                        command.Parameters.AddWithValue("@AddressLine1", billingDetail.AddressLine1);
                        command.Parameters.AddWithValue("@AddressLine2", (string.IsNullOrEmpty(billingDetail.AddressLine2) ? string.Empty : billingDetail.AddressLine2));
                        command.Parameters.AddWithValue("@AddressLine3", (string.IsNullOrEmpty(billingDetail.AddressLine3) ? string.Empty : billingDetail.AddressLine3));
                        command.Parameters.AddWithValue("@City", billingDetail.City);
                        command.Parameters.AddWithValue("@BillingPhone", billingDetail.BillingPhone);
                        command.Parameters.Add("@ErrorMsg", SqlDbType.VarChar, 500);
                        command.Parameters["@ErrorMsg"].Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        string errorMsg = Convert.ToString(command.Parameters["@ErrorMsg"].Value);
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            isUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static bool UpdateFlightPriceDetails(FlightPriceDetailsViewModel priceDetail, string connectionString)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "dbo.FlightPriceDetailsUpdate";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@Id", priceDetail.Id);
                        command.Parameters.AddWithValue("@BookingId", priceDetail.BookingId);
                        command.Parameters.AddWithValue("@FareBaseCode", !string.IsNullOrEmpty(priceDetail.FareBaseCode) ? priceDetail.FareBaseCode : string.Empty);
                        command.Parameters.AddWithValue("@PaxType", priceDetail.PaxType);
                        command.Parameters.AddWithValue("@Currency", priceDetail.Currency);
                        command.Parameters.AddWithValue("@PaxCount", priceDetail.PaxCount);
                        command.Parameters.AddWithValue("@BaseFare", priceDetail.BaseFare);
                        command.Parameters.AddWithValue("@Tax", priceDetail.Tax);
                        command.Parameters.AddWithValue("@Markup", priceDetail.Markup);
                        command.Parameters.AddWithValue("@SupplierFee", priceDetail.SupplierFee);
                        command.Parameters.AddWithValue("@Discount", priceDetail.Discount);
                        command.Parameters.AddWithValue("@IsSellInsurance", priceDetail.IsSellInsurance);
                        command.Parameters.AddWithValue("@InsuranceAmount", priceDetail.InsuranceAmount);
                        command.Parameters.AddWithValue("@IsSellBaggageInsurance", priceDetail.IsSellBaggageInsurance);
                        command.Parameters.AddWithValue("@BaggageInsuranceAmount", priceDetail.BaggageInsuranceAmount);
                        command.Parameters.AddWithValue("@TotalAmount", priceDetail.GetTotal);
                        command.Parameters.AddWithValue("@IsExtendedCancellation", priceDetail.IsExtendedCancellation);
                        command.Parameters.AddWithValue("@ExtendedCancellationAmount", priceDetail.ExtendedCancellationAmount);
                        command.Parameters.AddWithValue("@BookingFee", priceDetail.BookingFee);
                        command.ExecuteNonQuery();

                        isUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static List<SQL.FlightSegments> GetFlightSegmentDetails(int bookingId, string connectionString)
        {
            SqlDataReader dataReader = null;
            SQL.FlightSegments segment = null;
            List<SQL.FlightSegments> lstFlightSegment = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.FlightSegmentDetailsGet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingId ", bookingId);
                    con.Open();

                    dataReader = cmd.ExecuteReader();

                    /* Read data from FlightSegments table */
                    lstFlightSegment = new List<SQL.FlightSegments>();
                    while (dataReader.Read())
                    {
                        segment = new SQL.FlightSegments();

                        segment.Id = dataReader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["Id"]);
                        segment.BookingId = dataReader["BookingId"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["BookingId"]);
                        segment.SegmentOrder = dataReader["SegmentOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["SegmentOrder"]);
                        segment.IsReturn = dataReader["IsReturn"] == DBNull.Value ? false : Convert.ToBoolean(dataReader["IsReturn"]);
                        segment.FlightNumber = dataReader["FlightNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["FlightNumber"]);
                        //segment.ValAirlineCode = dataReader["ValAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["ValAirlineCode"]);
                        segment.MktAirlineCode = dataReader["MktAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["MktAirlineCode"]);
                        segment.OptAirlineCode = dataReader["OptAirlineCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["OptAirlineCode"]);
                        segment.OriginCode = dataReader["OriginCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["OriginCode"]);
                        segment.DeptDateTime = Convert.ToDateTime(dataReader["DeptDateTime"]);
                        segment.DeptTerminal = dataReader["DeptTerminal"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["DeptTerminal"]);
                        segment.DestinationCode = dataReader["DestinationCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["DestinationCode"]);
                        segment.ArrivalDateTime = Convert.ToDateTime(dataReader["ArrivalDateTime"]);
                        segment.ArrivalTerminal = dataReader["ArrivalTerminal"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["ArrivalTerminal"]);
                        segment.EquipmentDetail = dataReader["EquipmentDetail"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["EquipmentDetail"]);
                        segment.SegmentClass = dataReader["SegmentClass"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["SegmentClass"]);
                        segment.Stops = dataReader["Stops"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["Stops"]);
                        segment.Cabin = dataReader["Cabin"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["Cabin"]);
                        segment.CompanyFranchiseDetails = dataReader["CompanyFranchiseDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["CompanyFranchiseDetails"]);
                        segment.TechnicalStoppages = dataReader["TechnicalStoppages"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["TechnicalStoppages"]);
                        segment.AirlineLocator = dataReader["AirlineLocator"] == DBNull.Value ? string.Empty : Convert.ToString(dataReader["AirlineLocator"]);
                        segment.Created = DateTime.SpecifyKind(Convert.ToDateTime(dataReader["Created"]), DateTimeKind.Utc);

                        lstFlightSegment.Add(segment);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstFlightSegment;
        }
        public static bool UpdateTraveller(TravellersViewModel traveller, string connectionString)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "dbo.TravellerUpdate";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;

                        command.Parameters.AddWithValue("@Id", traveller.Id);
                        command.Parameters.AddWithValue("@BookingId", traveller.BookingId);
                        command.Parameters.AddWithValue("@Title", traveller.Title);
                        command.Parameters.AddWithValue("@FirstName", traveller.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", string.IsNullOrEmpty(traveller.MiddleName) ? string.Empty : traveller.MiddleName);
                        command.Parameters.AddWithValue("@LastName", traveller.LastName);
                        command.Parameters.AddWithValue("@Gender", traveller.Gender);
                        command.Parameters.AddWithValue("@DOB", traveller.DOB);
                        command.Parameters.AddWithValue("@PaxType", traveller.PaxType);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(traveller.Email) ? string.Empty : traveller.Email);
                        command.Parameters.AddWithValue("@TicketNo ", string.IsNullOrEmpty(traveller.TicketNo) ? string.Empty : traveller.TicketNo);
                        command.Parameters.AddWithValue("@PassportNumber ", string.IsNullOrEmpty(traveller.PassportNumber) ? string.Empty : traveller.PassportNumber);
                        command.Parameters.AddWithValue("@PassportIssuedBy ", string.IsNullOrEmpty(traveller.PassportIssuedBy) ? string.Empty : traveller.PassportIssuedBy);
                        command.Parameters.AddWithValue("@PassportExpireDate ", DBNull.Value);
                        command.ExecuteNonQuery();

                        isUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static bool UpdateFlightBookingSegments(List<SQL.FlightSegments> flightSegments, string conString)
        {
            bool isUpdated = false;
            SqlTransaction sqlTran = null;
            SqlConnection sqlConn = new SqlConnection(conString);


            try
            {
                if (flightSegments != null && flightSegments.Count > 0)
                {
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    sqlTran = sqlConn.BeginTransaction();

                    try
                    {
                        foreach (SQL.FlightSegments flightSeg in flightSegments)
                        {
                            UpdateFlightSegmentById(flightSeg, ref sqlConn, ref sqlTran);
                        }

                        sqlTran.Commit();
                        isUpdated = true;
                    }
                    catch (Exception)
                    {
                        sqlTran.Rollback();
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
                else
                {
                    isUpdated = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static bool UpdateFlightSegmentById(SQL.FlightSegments flightSegment, ref SqlConnection dbConn, ref SqlTransaction sqlTran)
        {
            bool isUpdated = false;

            try
            {
                using (SqlCommand command = new SqlCommand("", dbConn, sqlTran))
                {
                    command.CommandText = "dbo.FlightSegmentByIdUpdate";
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 3600;

                    command.Parameters.AddWithValue("@Id", flightSegment.Id);
                    command.Parameters.AddWithValue("@BookingId", flightSegment.BookingId);
                    command.Parameters.AddWithValue("@AirlineLocator", flightSegment.AirlineLocator);

                    command.ExecuteNonQuery();

                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static bool UpdateFlightBookingStatus(UpdateBookingStatusDetails _bookingStatus, string connectionString)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "dbo.FlightBookingStatusUpdate";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@BookingId", _bookingStatus.BookingId);
                        command.Parameters.AddWithValue("@BookingStatus", _bookingStatus.BookingStatus);
                        command.Parameters.AddWithValue("@Remarks", _bookingStatus.Remarks);
                        command.Parameters.AddWithValue("@UserId", _bookingStatus.UserId);
                        command.Parameters.AddWithValue("@UserName", _bookingStatus.UserName);
                        command.Parameters.Add("@ErrorMsg", SqlDbType.VarChar, 500);
                        command.Parameters["@ErrorMsg"].Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        string errorMsg = Convert.ToString(command.Parameters["@ErrorMsg"].Value);
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            isUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static bool BookingAssign(BookingAssignModelView _bookingStatus, string connectionString)
        {
            bool isUpdated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "dbo.BookingsAssign";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@UserId", _bookingStatus.UserId);
                        command.Parameters.AddWithValue("@BookingId", _bookingStatus.BookingId);
                        command.ExecuteNonQuery();
                        isUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        public static DashboardData GetDashboardData(string connectionString)
        {
            DashboardData response = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter sqlDataAdapter;
                    DataSet ds = new DataSet();
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "DashboardGet";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        sqlDataAdapter = new SqlDataAdapter(command);
                        sqlDataAdapter.Fill(ds);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            response = new DashboardData();

                            for (int i = 0; i <= ds.Tables.Count - 1; i++)
                            {
                                if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                foreach (DataRow row in ds.Tables[i].Rows)
                                                {
                                                    response.NewBooking = Convert.ToInt32(row["NewBooking"]);
                                                    response.Inprogress = Convert.ToInt32(row["Inprogress"]);
                                                    response.Completed = Convert.ToInt32(row["Completed"]);
                                                    response.TotalBooking = Convert.ToInt32(row["TotalBooking"]);
                                                    break;
                                                }
                                            }
                                            break;
                                        case 1:
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.MonthBooking = new MonthBooking();
                                                response.MonthBooking.Data = new List<DayCount>();
                                                foreach (DataRow dr in ds.Tables[i].Rows)
                                                {
                                                    DayCount dayCount = new DayCount();
                                                    //DataRow dr = ds.Tables[i].Rows[c - 1];
                                                    if (dr!=null && dr["Day"]!= DBNull.Value)
                                                    {
                                                        dayCount.Day = Convert.ToInt32(dr["Day"]);
                                                        if(dr["Count"] != DBNull.Value)
                                                        {
                                                            dayCount.Count = Convert.ToInt32(dr["Count"]);
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    dayCount.Day = c;
                                                    //    dayCount.Count = 0;
                                                    //}
                                                    response.MonthBooking.Data.Add(dayCount);

                                                }
                                            }
                                            break;
                                        case 2:
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.MonthSearch = new MonthSearch();
                                                response.MonthSearch.Data = new List<DayCount>();
                                                foreach (DataRow dr in ds.Tables[i].Rows)
                                                {
                                                    DayCount dayCount = new DayCount();
                                                    //DataRow dr = ds.Tables[i].Rows[c - 1];
                                                    if (dr != null && dr["Day"] != DBNull.Value)
                                                    {
                                                        dayCount.Day = Convert.ToInt32(dr["Day"]);
                                                        if (dr["Count"] != DBNull.Value)
                                                        {
                                                            dayCount.Count = Convert.ToInt32(dr["Count"]);
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    dayCount.Day = c;
                                                    //    dayCount.Count = 0;
                                                    //}
                                                    response.MonthSearch.Data.Add(dayCount);

                                                }
                                            }
                                            break;
                                        case 3: 
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.RouteSearch = new RouteSearch();
                                                response.RouteSearch.Data = new List<RouteCount>();
                                                foreach (DataRow dr in ds.Tables[i].Rows)
                                                {
                                                    if (dr != null && dr["Route"] != DBNull.Value)
                                                    {
                                                        RouteCount routCount = new RouteCount();
                                                        routCount.Route = Convert.ToString(dr["Route"]);
                                                        if (dr["Count"] != DBNull.Value)
                                                        {
                                                            routCount.Count = Convert.ToInt32(dr["Count"]);
                                                        }
                                                        else
                                                        {
                                                            routCount.Count = 0;
                                                        }
                                                        response.RouteSearch.Data.Add(routCount);
                                                    }
                                                }
                                            }
                                            break;
                                        case 4:
                                            if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                                            {
                                                response.AffiliateSearches = new AffiliateSearches();
                                                response.AffiliateSearches.Affiliates = new List<AffiliateSearch>();
                                                foreach (DataRow dr in ds.Tables[i].Rows)
                                                {
                                                    if (dr != null && dr["AffiliateId"] != DBNull.Value)
                                                    {
                                                        AffiliateSearch routCount = new AffiliateSearch();
                                                        routCount.Afffliate = (AffiliateType)Convert.ToInt32(dr["AffiliateId"]);
                                                        if (dr["Count"] != DBNull.Value)
                                                        {
                                                            routCount.Count = Convert.ToInt32(dr["Count"]);
                                                        }
                                                        else
                                                        {
                                                            routCount.Count = 0;
                                                        }
                                                        response.AffiliateSearches.Affiliates.Add(routCount);
                                                    }
                                                }
                                            }
                                            break;

                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public static int SaveBookingDetails(Dictionary<string, DataTable> bookingDetail, string connectionString)
        {
            int tid = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "usp_SaveBookingDetails";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@tblBookings", bookingDetail["Bookings"]);
                        command.Parameters.AddWithValue("@tblFlights", bookingDetail["Flights"]);
                        command.Parameters.AddWithValue("@tblSegments", bookingDetail["Segments"]);
                        command.Parameters.AddWithValue("@tblTravellers", bookingDetail["Travellers"]);
                        command.Parameters.AddWithValue("@tblPriceDetails", bookingDetail["PriceDetails"]);
                        command.Parameters.AddWithValue("@tblBillingDetails", bookingDetail["BillingDetails"]);
                        command.Parameters.AddWithValue("@tblBookingExtras", bookingDetail["BookingExtras"]);
                        tid = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tid;
        }
        public static void UpdateMarkupsLive(DataTable _markupIds, string _connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "MarkupsDoneLive";
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 3600;
                        command.Parameters.AddWithValue("@tblMarkupsIds", _markupIds);
                        command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
