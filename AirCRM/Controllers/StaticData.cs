//using Common;
using Infrastructure;
using TravelCRM.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Configuration;
using Infrastructure.HelpingModels;
using Business;

namespace TravelCRM.Controllers
{
    public class StaticData
    {
        public static List<SelectListItem> GetBookingSearchType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Search By", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "PNR", Value = ((int)BookingSearchType.PNR).ToString() });
            items.Add(new SelectListItem { Text = "Booking Number", Value = ((int)BookingSearchType.BookingNumber).ToString() });
            items.Add(new SelectListItem { Text = "Email", Value = ((int)BookingSearchType.Email).ToString() });
            items.Add(new SelectListItem { Text = "Contact Number", Value = ((int)BookingSearchType.ContactNumber).ToString() });
            items.Add(new SelectListItem { Text = "First Name", Value = ((int)BookingSearchType.FirstName).ToString() });
            items.Add(new SelectListItem { Text = "Last Name", Value = ((int)BookingSearchType.LastName).ToString() });
            items.Add(new SelectListItem { Text = "Card Holder Name", Value = ((int)BookingSearchType.CardHolderName).ToString() });
            return items;
        }
        public static List<SelectListItem> GetBookingSourceType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Source By", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Online", Value = ((int)BookingSourceType.OnlineBooking).ToString() });
            items.Add(new SelectListItem { Text = "Offline", Value = ((int)BookingSourceType.OfflineBooking).ToString() });
            return items;
        }
        public static List<SelectListItem> GetProviderType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Provider Type", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Amadeus Self Service", Value = ((int)ProviderType.AMADEUSSELFSERVICE).ToString() });
            return items;
        }
        public static List<SelectListItem> GetPortalType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Portal", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "FlightsChoice", Value = ((int)PortalType.FlightsChoice).ToString() });
            
            return items;
        }
        public static List<SelectListItem> FlightSearchType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "With Result", Value = "1" });
            items.Add(new SelectListItem { Text = "Without Result", Value = "0"});
            return items;
        }
        public static List<SelectListItem> GetPaxType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select pax type", Value = "0" });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TravellerPaxType)(int)TravellerPaxType.ADT), Value = TravellerPaxType.ADT.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TravellerPaxType)(int)TravellerPaxType.SEN), Value = TravellerPaxType.SEN.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TravellerPaxType)(int)TravellerPaxType.CHD), Value = TravellerPaxType.CHD.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TravellerPaxType)(int)TravellerPaxType.INS), Value = TravellerPaxType.INS.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TravellerPaxType)(int)TravellerPaxType.INL), Value = TravellerPaxType.INL.ToString() });
            return items;
        }
        public static List<SelectListItem> GetBookingStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Status", Value = "0" });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((BookingStatus)(int)BookingStatus.Pending), Value = BookingStatus.Pending.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((BookingStatus)(int)BookingStatus.InProgress), Value = BookingStatus.InProgress.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((BookingStatus)(int)BookingStatus.Completed), Value = BookingStatus.Completed.ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((BookingStatus)(int)BookingStatus.Cancelled), Value = BookingStatus.Cancelled.ToString() });
            return items;
        }
        
        public static bool DisplayLinkAsPerRole(List<string> roles)
        {
            bool response = false;
            try
            {
                foreach (var item in roles)
                {
                    response = UserSessionsHandler.LoggedInUser.Roles.Any(o => o.Equals(item));
                    if (response)
                        break;
                }
                
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Common.Utility.DisplayLinkAsPerRole:" + ex.ToString());
            }
            return response;
        }
        public static List<SelectListItem> GetMonths(int _month = 0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Month", Value = "0", Selected = _month == 0 });
            items.Add(new SelectListItem { Text = "JAN", Value = "1", Selected = _month == 1 });
            items.Add(new SelectListItem { Text = "FEB", Value = "2", Selected = _month == 2 });
            items.Add(new SelectListItem { Text = "MAR", Value = "3", Selected = _month == 3 });
            items.Add(new SelectListItem { Text = "APR", Value = "4", Selected = _month == 4 });
            items.Add(new SelectListItem { Text = "MAY", Value = "5", Selected = _month == 5 });
            items.Add(new SelectListItem { Text = "JUN", Value = "6", Selected = _month == 6 });
            items.Add(new SelectListItem { Text = "JUL", Value = "7", Selected = _month == 7 });
            items.Add(new SelectListItem { Text = "AUG", Value = "8", Selected = _month == 8 });
            items.Add(new SelectListItem { Text = "SEP", Value = "9", Selected = _month == 9 });
            items.Add(new SelectListItem { Text = "OCT", Value = "10", Selected = _month == 10 });
            items.Add(new SelectListItem { Text = "NOV", Value = "11", Selected = _month == 11 });
            items.Add(new SelectListItem { Text = "DEC", Value = "12", Selected = _month == 12 });
            return items;
        }
        public static List<SelectListItem> GetPaymentMethod(int _cardType = 0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select payment method", Value = "0", Selected = _cardType == (int)PaymentMethod.None });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.Visa), Value = ((int)PaymentMethod.Visa).ToString(), Selected = _cardType == (int)PaymentMethod.Visa });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.MasterCard), Value = ((int)PaymentMethod.MasterCard).ToString(), Selected = _cardType == (int)PaymentMethod.MasterCard });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.AmericanExpress), Value = ((int)PaymentMethod.AmericanExpress).ToString(), Selected = _cardType == (int)PaymentMethod.AmericanExpress });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.DinersClub), Value = ((int)PaymentMethod.DinersClub).ToString(), Selected = _cardType == (int)PaymentMethod.DinersClub });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.Discover), Value = ((int)PaymentMethod.Discover).ToString(), Selected = _cardType == (int)PaymentMethod.Discover });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.Electron), Value = ((int)PaymentMethod.Electron).ToString(), Selected = _cardType == (int)PaymentMethod.Electron });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.Maestro), Value = ((int)PaymentMethod.Maestro).ToString(), Selected = _cardType == (int)PaymentMethod.Maestro });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.BCCard), Value = ((int)PaymentMethod.BCCard).ToString(), Selected = _cardType == (int)PaymentMethod.BCCard });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription(PaymentMethod.JCB), Value = ((int)PaymentMethod.JCB).ToString(), Selected = _cardType == (int)PaymentMethod.JCB });

            return items;
        }
        public static List<SelectListItem> GetUsers()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select user", Value = "0", Selected = true });
            try
            {
                List<UserData> users = BookingBusiness.GetUsers(null);
                if (users != null && users.Count > 0)
                {
                    foreach (var item in users)
                    {
                        items.Add(new SelectListItem { Text = string.Format("{0} {1}-({2})", item.FirstName, item.LastName, item.UserName) , Value = item.UserId.ToString() });
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return items;
        }

        public static List<SelectListItem> GetCountries()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (Country item in Utility.Settings.Country)
            {
                if (item.Code.Equals("US", StringComparison.OrdinalIgnoreCase))
                {
                    items.Add(new SelectListItem { Text = item.Name, Value = item.Code, Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = item.Name, Value = item.Code });
                }

            }

            return items;
        }
        public static List<SelectListItem> GetStates(string _countryCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<State> states = Utility.GetStates(_countryCode);
            foreach (State item in states)
            {
                items.Add(new SelectListItem { Text = item.Name, Value = item.Code });
            }
            return items;
        }
        public static List<SelectListItem> GetTitles(int _title = 0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Title", Value = "0", Selected = _title == (int)TravellerTitleType.None });
            items.Add(new SelectListItem { Text = "Mr.", Value = ((int)TravellerTitleType.MR).ToString(), Selected = _title == (int)TravellerTitleType.MR });
            items.Add(new SelectListItem { Text = "Ms.", Value = ((int)TravellerTitleType.MS).ToString(), Selected = _title == (int)TravellerTitleType.MS });
            items.Add(new SelectListItem { Text = "Mrs.", Value = ((int)TravellerTitleType.MRS).ToString(), Selected = _title == (int)TravellerTitleType.MRS });

            return items;
        }
        public static List<SelectListItem> GetGenders(int _gender = 0)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Gender", Value = "0", Selected = _gender == 0 });
            items.Add(new SelectListItem { Text = "Male", Value = "1", Selected = _gender == 1 });
            items.Add(new SelectListItem { Text = "Female", Value = "2", Selected = _gender == 2 });
            return items;
        }
        public static List<SelectListItem> GetYears()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Year", Value = "0", Selected = true });
            int year = DateTime.Now.Year;
            for (int i = year; i <= year + 10; i++)
            {
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

            }
            return items;
        }
        public static List<SelectListItem> GetTripType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select trip type", Value = "0" });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TripType)(int)TripType.ONEWAY), Value = ((int)TripType.ONEWAY).ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((TripType)(int)TripType.ROUNDTRIP), Value = ((int)TripType.ROUNDTRIP).ToString() });
            return items;
        }
        public static List<SelectListItem> GetCabins()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((CabinType)(int)CabinType.Economy), Value = ((int)CabinType.Economy).ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((CabinType)(int)CabinType.Business), Value = ((int)CabinType.Business).ToString() });
            items.Add(new SelectListItem { Text = Utility.GetEnumDescription((CabinType)(int)CabinType.First), Value = ((int)CabinType.First).ToString() });
            return items;
        }
        public static List<SelectListItem> GetMarkupType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = MarkupType.Amount.ToString(), Value = ((int)MarkupType.Amount).ToString() });
            items.Add(new SelectListItem { Text = MarkupType.Percentage.ToString(), Value = ((int)MarkupType.Percentage).ToString() });
            return items;
        }
        public static List<SelectListItem> GetRoutesMarkup()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = RouteType.Domestic.ToString(), Value = ((int)RouteType.Domestic).ToString() });
            items.Add(new SelectListItem { Text = RouteType.International.ToString(), Value = ((int)RouteType.International).ToString() });
            return items;
        }
        public static List<SelectListItem> GetFareType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Fare Type", Value = "0" });
            items.Add(new SelectListItem { Text = FareType.PRIVATE.ToString(), Value = ((int)FareType.PRIVATE).ToString() });
            items.Add(new SelectListItem { Text = FareType.PRIVATE.ToString(), Value = ((int)FareType.PRIVATE).ToString() });
            items.Add(new SelectListItem { Text = FareType.PUBLISHED.ToString(), Value = ((int)FareType.PUBLISHED).ToString() });
            return items;
        }
        public static List<SelectListItem> GetWeekDays()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = (DayOfWeek.Sunday).ToString(), Value = ((int)DayOfWeek.Sunday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Monday).ToString(), Value = ((int)DayOfWeek.Monday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Tuesday).ToString(), Value = ((int)DayOfWeek.Tuesday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Wednesday).ToString(), Value = ((int)DayOfWeek.Wednesday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Thursday).ToString(), Value = ((int)DayOfWeek.Thursday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Friday).ToString(), Value = ((int)DayOfWeek.Friday).ToString() });
            items.Add(new SelectListItem { Text = (DayOfWeek.Saturday).ToString(), Value = ((int)DayOfWeek.Saturday).ToString() });
            return items;
        }
        public static List<SelectListItem> GetDiscountType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = DiscountType.Percentage.ToString(), Value = ((int)DiscountType.Percentage).ToString() });
            items.Add(new SelectListItem { Text = DiscountType.Amount.ToString(), Value = ((int)DiscountType.Amount).ToString() });            
            return items;
        }
    }
}