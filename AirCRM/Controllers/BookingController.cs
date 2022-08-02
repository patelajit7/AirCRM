
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Infrastructure.HelpingModel;
using Common;
using Presentation.Controllers;
using System.Globalization;
using Infrastructure;
using Infrastructure.HelpingModels;
using Business;
using TravelCRM.Authentication;
using Infrastructure.HelpingModel.BookingEntities;
using Infrastructure.Entities;
using System.Linq;
using SQL = Infrastructure.Entities;
using Infrastructure.HelpingModels.ViewModel;
using System.Threading.Tasks;
using System.Web;

namespace TravelCRM.Controllers
{
    [UserAuthorization]
    public class BookingController : Controller
    {
        [Route("bookings-Search")]
        [RoleAuthorize("Admin", "Agent", "Supervisor")]
        [HttpGet]
        public ActionResult Bookings()
        {
            return View();
        }

        [HttpPost]
        [RoleAuthorize("Admin", "Agent", "Supervisor")]
        public ActionResult OpenBooking(FormCollection collection)
        {
            BookingDetails booking = null;
            try
            {
                if (collection != null)
                {
                    int bookingId = 0;
                    bookingId = Convert.ToInt32(collection["BookingId"]);
                    if (bookingId > 0)
                    {
                        booking = BookingBusiness.GetBookingDetails(bookingId);
                        string fName = UserSessionsHandler.LoggedInUser.FirstName;
                        string lName = UserSessionsHandler.LoggedInUser.LastName;
                        int userId = UserSessionsHandler.LoggedInUser.UserId;
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                SQL.FlightBookingRemarks remarks = new SQL.FlightBookingRemarks()
                                {
                                    BookingId = bookingId,
                                    UserName = string.Format("{0} {1}", fName, lName),
                                    UserId = userId,
                                    Remark = string.Format("Booking is viewd by {0} {1} at {2}", fName, lName, DateTime.UtcNow.ToString("dd MMM yyy HH:mm:ss")),
                                    Created = DateTime.UtcNow
                                };
                                Utility.DatabaseService.Create<SQL.FlightBookingRemarks>(remarks);
                            }
                            catch (Exception ex)
                            {

                            }

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("BookingsController.OpenBooking|Exception: " + ex.ToString());
            }

            return View(booking);
        }

        [Route("bookings-search-by-type")]
        [RoleAuthorize("Admin", "Agent", "Supervisor")]
        [HttpPost]
        public ActionResult SearchBookingBasedOnSearchType(int searchParam, string searchValue)
        {
            List<FlightBooking> lstBookingsInfo = null;

            try
            {
                lstBookingsInfo = BookingBusiness.GetBookingsBasedSearchType(searchParam, searchValue);

                var result = new
                {
                    isSuccess = (lstBookingsInfo != null && lstBookingsInfo.Count > 0) ? true : false,
                    message = (lstBookingsInfo != null && lstBookingsInfo.Count > 0) ? "Success" : "No booking record found !",
                    hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_searchBooking.cshtml", lstBookingsInfo)
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("TravelCRM.BookingsController.SearchBookingBasedOnSearchType|Exception: " + ex.ToString());
                return View();
            }
        }

        [Route("bookings-search-search-dates")]
        [RoleAuthorize("Admin", "Agent", "Supervisor")]
        [HttpPost]
        public ActionResult SearchBookingDetailsByDate(string _startDate, string _endDate, int _bookingSubStatus, int _dateSearchType, bool _isOnline)
        {
            List<FlightBooking> lstBookingsInfo = null;

            try
            {
                DateTime startDate = DateTime.ParseExact(_startDate, "MM/dd/yyyy", new CultureInfo("en-US", false), DateTimeStyles.None);
                DateTime endDate = DateTime.ParseExact(_endDate, "MM/dd/yyyy", new CultureInfo("en-US", false), DateTimeStyles.None);
                string errMsg = string.Empty;
                int days = startDate.CompareTo(endDate);
                if (_bookingSubStatus == -1)
                {
                    if (days > 0)
                    {
                        errMsg = "<b>To</b> date must be greater than or equal to <b>From</b> date.";
                    }
                }


                if (string.IsNullOrEmpty(errMsg))
                {
                    SearchCriteria searchCriteria = new SearchCriteria()
                    {
                        From = startDate,
                        End = endDate,
                        Status = _bookingSubStatus,
                        SearchType = (BookingSearchType)_dateSearchType,
                        IsOnlineBookings = true
                    };

                    lstBookingsInfo = BookingBusiness.GetBookingsBasedDates(searchCriteria);

                    var result = new
                    {
                        isSuccess = (lstBookingsInfo != null && lstBookingsInfo.Count > 0) ? true : false,
                        isValidation = false,
                        message = (lstBookingsInfo != null && lstBookingsInfo.Count > 0) ? "Success" : "No booking record found !",
                        hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_searchBooking.cshtml", lstBookingsInfo)
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { isSuccess = false, isValidation = true, message = errMsg });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("TravelCRM.BookingsController.SearchBookingDetailsByDate|Exception: " + ex.ToString());
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("booking/edit-booking-detail")]
        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult EditBookingDetail(int bookingId)
        {
            BookingsViewModel response = null;
            try
            {
                SQL.Bookings bookingDetail = Utility.DatabaseService.Where<SQL.Bookings>(x => x.Id == bookingId).ToList<SQL.Bookings>().FirstOrDefault();
                if (bookingDetail != null)
                {
                    response = new BookingsViewModel() { PNR = bookingDetail.PNR, AirlinePNR = bookingDetail.AirlinePNR, PortalId = bookingDetail.PortalId, ProviderId = bookingDetail.ProviderId, BookingSourceType = bookingDetail.BookingSourceType };
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BookingDetailsEdit.cshtml", response);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Booking details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Bookings.Controllers.BookingsController.EditBookingDetail|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult UpdateBooking(BookingsViewModel model)
        {
            try
            {
                if (model != null)
                {
                    bool isUpdated = BookingBusiness.UpdateBookings(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Booking details updated sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Booking details updation failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Booking details not found to upadate." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Bookings.Controllers.BookingsController.UpdateBooking|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking details updation failed. Something going wrong." });
            }
        }

        [Route("booking/edit-billing-detail")]
        [RoleAuthorize("Admin")]
        [HttpPost]
        public ActionResult EditBillingDetail(int id)
        {
            BillingDetailsViewModel billingDetail = null;
            try
            {
                billingDetail = BookingBusiness.GetBillingDetailsById(id);
                if (billingDetail != null)
                {
                    TempData["CardNo"] = billingDetail.CardNumber;
                    TempData["BillPh"] = billingDetail.BillingPhone;
                    TempData["ContPh"] = billingDetail.ContactPhone;
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BillingDetailsEdit.cshtml", billingDetail);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Billing details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.EditBillingDetail|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult UpdateBillingDetail(BillingDetailsViewModel model)
        {
            BillingDetailsViewModel billingDetail = null;
            try
            {
                if (model != null)
                {

                    if (string.IsNullOrEmpty(model.CardNumber) || string.IsNullOrEmpty(model.BillingPhone) || string.IsNullOrEmpty(model.ContactPhone))
                    {
                        billingDetail = BookingBusiness.GetBillingDetailsById(model.BookingId);
                        if (billingDetail != null)
                        {
                            if (string.IsNullOrEmpty(model.CardNumber)) model.CardNumber = billingDetail.CardNumber;
                            if (model.CountryCode == "-999") model.CountryCode = string.Empty;
                            if (string.IsNullOrEmpty(model.AreaCode)) model.AreaCode = string.Empty;
                            if (string.IsNullOrEmpty(model.BillingPhone)) model.BillingPhone = billingDetail.BillingPhone;
                            if (string.IsNullOrEmpty(model.ContactPhone)) model.ContactPhone = billingDetail.ContactPhone;
                        }
                    }

                    bool isUpdated = BookingBusiness.UpdateBillingDetails(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Billing details updated sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Billing details updation failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Billing details not found to upadate." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.UpdateBillingDetail|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Billing details updation failed. Something going wrong." });
            }
        }
        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult EditPriceDetail(int id)
        {
            FlightPriceDetailsViewModel response = null;

            try
            {
                SQL.FlightPriceDetails priceDetail = Utility.DatabaseService.Where<SQL.FlightPriceDetails>(x => x.Id == id).ToList<SQL.FlightPriceDetails>().FirstOrDefault();

                if (priceDetail != null)
                {
                    response = new FlightPriceDetailsViewModel()
                    {
                        Id = priceDetail.Id,
                        BookingId = priceDetail.BookingId,
                        FareBaseCode = priceDetail.FareBaseCode,
                        PaxType = priceDetail.PaxType,
                        Currency = priceDetail.Currency,
                        PaxCount = priceDetail.PaxCount,
                        BaseFare = priceDetail.BaseFare,
                        Tax = priceDetail.Tax,
                        Markup = priceDetail.Markup,
                        SupplierFee = priceDetail.SupplierFee,
                        Discount = priceDetail.Discount,
                        IsSellInsurance = priceDetail.IsSellInsurance,
                        InsuranceAmount = priceDetail.InsuranceAmount,
                        IsSellBaggageInsurance = priceDetail.IsSellBaggageInsurance,
                        BaggageInsuranceAmount = priceDetail.BaggageInsuranceAmount,
                        TotalAmount = priceDetail.TotalAmount,
                        IsExtendedCancellation = priceDetail.IsExtendedCancellation,
                        ExtendedCancellationAmount = priceDetail.ExtendedCancellationAmount,
                        BookingFee = priceDetail.BookingFee
                    };
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_PriceDetailsEdit.cshtml", response);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Price details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.EditPriceDetail|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult UpdateFlightPriceDetail(FlightPriceDetailsViewModel model)
        {
            try
            {
                if (model != null)
                {
                    bool isUpdated = BookingBusiness.UpdateFlightPriceDetails(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Price details updated sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Price details updation failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Price details not found to upadate." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.UpdateFlightPriceDetail|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Price details updation failed. Something going wrong." });
            }
        }
        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult EditFlightTraveller(int id)
        {
            TravellersViewModel response = null;
            try
            {
                SQL.Travellers travellerDetail = Utility.DatabaseService.Where<SQL.Travellers>(x => x.Id == id).ToList<SQL.Travellers>().FirstOrDefault();
                if (travellerDetail != null)
                {
                    response = new TravellersViewModel()
                    {
                        Id = travellerDetail.Id,
                        BookingId = travellerDetail.BookingId,
                        Title = travellerDetail.Title,
                        FirstName = travellerDetail.FirstName,
                        MiddleName = travellerDetail.MiddleName,
                        LastName = travellerDetail.LastName,
                        DOB = travellerDetail.DOB,
                        Gender = travellerDetail.Gender,
                        Email = travellerDetail.Email,
                        PaxOrder = travellerDetail.PaxOrder,
                        PaxType = travellerDetail.PaxType,

                    };

                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_TravellerDetailsEdit.cshtml", response);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.EditFlightTraveller|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult ViewFlightTraveller(int id)
        {
            SQL.Travellers travellerDetail = null;
            try
            {
                travellerDetail = Utility.DatabaseService.Where<SQL.Travellers>(x => x.Id == id).ToList<SQL.Travellers>().FirstOrDefault();
                if (travellerDetail != null)
                {
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_FlightTravellerDetailsView.cshtml", travellerDetail);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.ViewFlightTraveller|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateFlightTraveller(TravellersViewModel model)
        {
            try
            {
                if (model != null)
                {
                    bool isUpdated = BookingBusiness.UpdateTraveller(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Traveller details updated sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Traveller details updation failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found to upadate." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Business.Controllers.BookingsController.UpdateFlightTraveller|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Traveller details updation failed. Something going wrong." });
            }
        }


        [HttpPost]
        public ActionResult DeleteFlightTravellerConfirmation(int id)
        {
            SQL.Travellers travellerDetail = null;
            try
            {
                travellerDetail = Utility.DatabaseService.Where<SQL.Travellers>(x => x.Id == id).ToList<SQL.Travellers>().FirstOrDefault();
                if (travellerDetail != null)
                {
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_FlightTravellerDelete.cshtml", travellerDetail);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.DeleteFlightTravellerConfirmation|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteFlightTraveller(int id, int bookingId)
        {
            bool isUpdated = false;
            try
            {
                if (id > 0)
                {
                    isUpdated = BookingBusiness.DeleteFlightTraveller(id);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Traveller details deleted sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Traveller details deletion failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found to delete." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.DeleteFlightTraveller|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Traveller details deletion failed. Something going wrong." });
            }
        }

        [HttpPost]
        public ActionResult AddNewFlightTraveller()
        {
            try
            {
                SQL.Travellers travellerDetail = new SQL.Travellers();
                string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_AddNewFlightTraveller.cshtml", travellerDetail);
                return Json(new { isSuccess = true, html = hTMLString });
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.AddNewFlightTraveller|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveNewFlightTraveller(SQL.Travellers model, string dob, string passExpDate)
        {
            try
            {
                if (model != null)
                {
                    model.DOB = DateTime.ParseExact(dob, "MM-dd-yyyy", new CultureInfo("en-US", false), DateTimeStyles.None);
                    int id = Utility.DatabaseService.Create<SQL.Travellers>(model);
                    if (id > 0)
                    {
                        return Json(new { isSuccess = true, message = "Traveller details saved sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Traveller details saving failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Traveller details not found to save." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.SaveNewFlightTraveller|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Traveller details saving failed. Something going wrong." });
            }
        }
        [HttpPost]
        public ActionResult EditFlightBookingSegments(int bookingId)
        {
            List<SQL.FlightSegments> segments = null;
            FlightSegmentDetailToUpdate flightSegmentDetail = new FlightSegmentDetailToUpdate();

            try
            {
                segments = BookingBusiness.GetFlightSegmentDetails(bookingId);

                if (segments != null && segments.Count > 0)
                {
                    flightSegmentDetail.Segments = new List<FlightSegmentDetail>();
                    foreach (SQL.FlightSegments item in segments)
                    {
                        FlightSegmentDetail flightSegs = new FlightSegmentDetail()
                        {
                            Id = item.Id,
                            BookingId = item.BookingId,
                            SegmentOrder = item.SegmentOrder,
                            FlightNumber = item.FlightNumber,
                            IsReturn = item.IsReturn,
                            OperatingCode = item.OptAirlineCode,
                            MarketingCode = item.MktAirlineCode,
                            Origin = item.OriginCode,
                            Destination = item.DestinationCode,
                            DeptDateTime = item.DeptDateTime,
                            DepartureTime = item.DeptDateTime.TimeOfDay,
                            ArrivalDateTime = item.ArrivalDateTime,
                            ArrivalTime = item.ArrivalDateTime.TimeOfDay,
                            DeptTerminal = item.DeptTerminal,
                            ArrivalTerminal = item.ArrivalTerminal,
                            EquipmentDetail = item.EquipmentDetail,
                            SegmentClass = item.SegmentClass,
                            Stops = item.Stops,
                            Cabin = item.Cabin,
                            CompanyFranchiseDetails = item.CompanyFranchiseDetails,
                            TechnicalStoppages = item.TechnicalStoppages,
                            AirlineLocator = item.AirlineLocator,
                            RelatedProductStatus = item.SegmentType
                        };

                        flightSegmentDetail.Segments.Add(flightSegs);
                    }
                    string hTMLString = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_FlightBookingSegmentsEdit.cshtml", flightSegmentDetail);
                    return Json(new { isSuccess = true, html = hTMLString });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Flight segment details not found." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.EditFlightBookingSegments|Booking Id: " + bookingId + "|Exception: " + ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateFlightBoogingSegments(List<SQL.FlightSegments> model)
        {
            int bookingId = 0;
            bool isUpdated = false;
            try
            {
                if (model != null && model.Count > 0)
                {
                    bookingId = model.FirstOrDefault().BookingId;

                    isUpdated = BookingBusiness.UpdateFlightBookingSegments(model);

                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Flight booking segments updated successfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Flight booking segments updation failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Flight booking segments updation failed. Something going wrong." });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.UpdateFlightBoogingSegments | Booking Id: {0} | Exception: {1} ", bookingId, ex.ToString());
                return Json(new { isSuccess = false, message = "Flight booking segments updation failed. Something going wrong." });
            }
        }

        [Route("booking/booking-status-view")]
        [HttpPost]
        public ActionResult GetBookingStatusView(int id, string referenceNumber)
        {
            bool isUpdated = false;
            try
            {
                var result = new
                {
                    IsSuccess = true,
                    html = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BookingStatusUpdate.cshtml", new UpdateBookingStatusDetails() { BookingId = id, ReferenceId = referenceNumber })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.GetBookingStatusView|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking status updation failed. Something going wrong.", isStatusUpdated = isUpdated });
            }
        }

        [HttpPost]
        public ActionResult UpdateBookingStatus(UpdateBookingStatusDetails model)
        {
            bool isUpdated = false;
            try
            {
                if (model != null)
                {
                    string fName = UserSessionsHandler.LoggedInUser.FirstName;
                    string lName = UserSessionsHandler.LoggedInUser.LastName;
                    int userId = UserSessionsHandler.LoggedInUser.UserId;

                    model.UserId = UserSessionsHandler.LoggedInUser.UserId;
                    model.UserName = string.Format("{0} {1}", fName, lName);
                    isUpdated = BookingBusiness.UpdateFlightBookingStatus(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Booking status updated sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Booking status updation failed. Something going wrong.", isStatusUpdated = isUpdated });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Booking status not found to upadate.", isStatusUpdated = isUpdated });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.UpdateBookingStatus|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking status updation failed. Something going wrong.", isStatusUpdated = isUpdated });
            }
        }

        [Route("booking/booking-remarks-view")]
        [HttpPost]
        public ActionResult GetBookingRemarksView(int id)
        {
            bool isUpdated = false;
            try
            {
                var result = new
                {
                    IsSuccess = true,
                    html = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BookingRemarksAdd.cshtml", new BookingRemarks() { BookingId = id })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.GetBookingStatusView|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking status updation failed. Something going wrong.", isStatusUpdated = isUpdated });
            }
        }

        [HttpPost]
        public ActionResult BookingRemarks(int id)
        {
            try
            {
                List<FlightBookingRemarks> lstFlightBookingRemarks = Utility.DatabaseService.Where<FlightBookingRemarks>(o => o.BookingId == id).OrderByDescending(o => o.Created).ToList<FlightBookingRemarks>();
                var result = new
                {
                    IsSuccess = true,
                    html = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BookingRemarksView.cshtml", lstFlightBookingRemarks)
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingController.BookingRemarks|Exception: " + ex.ToString());
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddBookingRemarks(BookingRemarks model)
        {
            try
            {
                SQL.FlightBookingRemarks remarks = new SQL.FlightBookingRemarks()
                {
                    BookingId = model.BookingId,
                    UserName = User.Identity.Name,
                    UserId = UserSessionsHandler.LoggedInUser.UserId,
                    Remark = model.Remarks,
                    Created = DateTime.UtcNow
                };
                Task.Factory.StartNew(() => Utility.DatabaseService.Create<SQL.FlightBookingRemarks>(remarks));

                List<FlightBookingRemarks> lstFlightBookingRemarks = Utility.DatabaseService.Where<FlightBookingRemarks>(o => o.BookingId == model.BookingId).OrderByDescending(o => o.Created).ToList<FlightBookingRemarks>();
                var result = new
                {
                    IsSuccess = true,
                    ResultList = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_FlightBookingRemarks.cshtml", lstFlightBookingRemarks)
                };
                return Json(result, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingController.BookingRemarks|Exception: " + ex.ToString());
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("booking/booking-assign-view")]
        [RoleAuthorize("Admin")]
        [HttpPost]
        public ActionResult GetBookingAssignView(int id)
        {
            bool isUpdated = false;
            try
            {
                var result = new
                {
                    isSuccess = true,
                    html = ShareUtility.RenderViewToString(this.ControllerContext, "~/Views/Booking/Partial/_BookingAssign.cshtml", new BookingAssignModelView() { BookingId = id })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.GetBookingAssignView|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking status updation failed. Something going wrong.", isStatusUpdated = isUpdated });
            }
        }

        [HttpPost]
        [RoleAuthorize("Admin")]
        public ActionResult BookingAssignToAgent(BookingAssignModelView model)
        {
            bool isUpdated = false;
            try
            {
                if (model != null)
                {
                    isUpdated = BookingBusiness.BookingAssign(model);
                    if (isUpdated)
                    {
                        return Json(new { isSuccess = true, message = "Booking assigned sucessfully." });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Booking assign failed. Something going wrong.", isStatusUpdated = isUpdated });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Bad request.", isStatusUpdated = isUpdated });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.BookingAssignToAgent|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking assign failed. Something going wrong.", isStatusUpdated = isUpdated });
            }
        }


        [HttpPost]
        public ActionResult SendEmailToCustomer(int id)
        {
            try
            {
                BookingDetails bookingDetails = null;
                if (id > 0)
                {
                    bookingDetails = BookingBusiness.GetBookingDetails(id);
                    if (bookingDetails != null)
                    {
                        string htmlMailString = ShareUtility.RenderViewToString(this.ControllerContext, "~/views/emails/bookingreceiptdb.cshtml", bookingDetails);

                        if (!string.IsNullOrEmpty(htmlMailString) && htmlMailString.Length > 1000)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                EmailTransaction transaction = new EmailTransaction()
                                {
                                    EmailType = EmailType.BookingReceipt,
                                    MailBody = htmlMailString,
                                    PortalId = bookingDetails.Transaction.PortalId,
                                    MailRecipient = bookingDetails.BillingDetails.Email,
                                    TransactionId = bookingDetails.Transaction.Id
                                };
                                bool isMailSent = EmailHelper.SendMails(transaction);
                                Utility.Logger.Info(string.Format("BOOKING RECEIPT|BookingId:{0}| TID:{1}", id, isMailSent ? "MAIL SENT" : "UNABLE TO SENT MAIL"));

                            });
                            return Json(new { isSuccess = true, message = "Email sent sucessfully to customer" });
                        }
                        else
                        {
                            return Json(new { isSuccess = false, message = "Email send failed. Something going wrong." });
                        }
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "Email send failed. Something going wrong." });
                    }
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Invalid booking detail" });
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("Booking.Controllers.BookingsController.SendEmailToCustomer|Exception: " + ex.ToString());
                return Json(new { isSuccess = false, message = "Booking details retrieved failed. Something going wrong." });
            }
            return Json(new { isSuccess = false, message = "Email send failed. Something going wrong." });
        }

    }
}