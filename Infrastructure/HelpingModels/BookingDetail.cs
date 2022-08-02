using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelpingModels
{
    public class BookingDetail
    {
        public Transaction Transaction { get; set; }
        public FlightSearch FlightSearch { get; set; }
        public Contract Contract { get; set; }
        public BillingDetail BillingDetails { get; set; }
        public List<Traveller> Travellers { get; set; }
        public BagInsuranc BagInsuranc { get; set; }
        public TravelerInsurance TravelerInsurance { get; set; }
        public Coupon CouponDetails { get; set; }
        public ExtendedCancellation ExtendedCancellation { get; set; }
        public float PriceIncrease { get; set; }        
        public CurrencyType Currency { get; set; }
        public float CurrencyConversion { get; set; }

    }
    public class Coupon
    {
        public string CouponCode { get; set; }
        public string CouponMessage { get; set; }
        public bool Status { get; set; }
        public float TotalAmount { get; set; }
    }
    public class BagInsuranc
    {
        public BagInsuranceType BagInsuranceType { get; set; }
        public decimal PPaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class TravelerInsurance
    {
        public bool IsTravelProtected { get; set; }
        public decimal PPaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string CardType { get; set; }
        public bool Error { get; set; }
        public string Warning { get; set; }

    }
    public class ExtendedCancellation
    {
        public bool IsExtendedCancellation { get; set; }
        public decimal PPaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class Transaction
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string PNR { get; set; }
        public string ReferenceNumber { get; set; }
        public int GDS { get; set; }
        public int ProviderId { get; set; }
        public int PortalId { get; set; }
        public int BookingType { get; set; }
        public int BookingStatus { get; set; }
        public int BookingSubStatus { get; set; }
        public int BookingSourceType { get; set; }
        public int AgentId { get; set; }
        public int AgentLead { get; set; }
        public int UserId { get; set; }
        public DateTime BookedOn { get; set; }
        public Transaction()
        {
            this.BookedOn = DateTime.UtcNow;
        }

    }
    public class FlightSearch : SearchUIExtend
    {
        public string SearchGuidId { get; set; }
        public int PortalId { get; set; }
        public int AffiliateId { get; set; }
        public TripType TripType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime? Return { get; set; }
        public int Adult { get; set; }
        public int Senior { get; set; }
        public int Child { get; set; }
        public int InfantOnSeat { get; set; }
        public int InfantOnLap { get; set; }
        public CabinType Cabin { get; set; }
        public string PreferredCarrier { get; set; }
        public bool IsDirectFlight { get; set; }
        public string IP { get; set; }
        public int TotalPax()
        {
            return this.Adult + this.Senior + this.Child + this.InfantOnSeat + this.InfantOnLap;
        }
        public bool IsMobileDevice { get; set; }
        public int UserId { get; set; }
        public string UserAgent { get; set; }
        public int ResultFound { get; set; }

    }
    public class SearchUIExtend
    {
        public string OriginSearch { get; set; }
        public string DestinationSearch { get; set; }
        public string OriginAirportName { get; set; }
        public string OriginCountry { get; set; }
        public string DestAirportName { get; set; }
        public string DestCountry { get; set; }
        public bool IsMetaSearch { get; set; }
        public DateTime Created { get; set; }
        /// <summary>
        /// Tracking property
        /// </summary>
        public string UtmSource { get; set; }
        public string UtmMedium { get; set; }
        public string UtmCampaign { get; set; }
        public string UtmTerm { get; set; }
        public string UtmContent { get; set; }
        public string UtmKeyword { get; set; }
        public string ClickedId { get; set; }
        public int PageId { get; set; }
        public DateTime SearchDateTime { get; set; }
        public string FlexiblityQualifier { get; set; }
    }
    public class Contract
    {
        public string SearchGuid { get; set; }
        public int ContractId { get; set; }
        public ProviderType Provider { get; set; }
        public GDSType GDSType { get; set; }
        public string Origin { get; set; }
        public string OriginCityName { get; set; }
        public string Destination { get; set; }
        public string DestinationCityName { get; set; }
        public string OriginSearch { get; set; }
        public string DestinationSearch { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public TripDetails TripDetails { get; set; }
        public Airline ValidatingCarrier { get; set; }
        public string FareType { get; set; }
        public TripType TripType { get; set; }
        public bool IsRefundable { get; set; }
        public int Adult { get; set; }
        public int Senior { get; set; }
        public int Child { get; set; }
        public int InfantOnSeat { get; set; }
        public int InfantOnLap { get; set; }
        public string FareBasisCode { get; set; }
        public FareDetails AdultFare { get; set; }
        public FareDetails ChildFare { get; set; }
        public FareDetails InfantOnSeatFare { get; set; }
        public FareDetails InfantOnLapFare { get; set; }
        public FareDetails SeniorFare { get; set; }
        public float TotalMarkup { get; set; }
        public float TotalSupplierFee { get; set; }
        public float TotalBaseFare { get; set; }
        public float TotalTax { get; set; }
        public float TotalGDSFareV2 { get; set; }
        public int EnginePriority { get; set; }
        public string Contractkey { get; set; }
        public string DatesKey { get; set; }



        public string PricingSource { get; set; }

        //public int MaxNoOfStopsInContract { get; set; }
        public int MaxStopOutbound { get; set; }
        public int MaxStopInbound { get; set; }
        public bool IsMultipleAirlineContract { get; set; }
        public int MinSeatAvailableForContract { get; set; }
        public bool IsPhoneOnly { get; set; }
        //Actual/NearBy/Flex
        public ContractType ContractType { get; set; }

        public TimeSpan TotalOutBoundFlightDuration { get; set; }
        public TimeSpan TotalInBoundFlightDuration { get; set; }
        public int AffiliateId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public float TotalBookingFee { get; set; }
        public int BaggageQuantity { get; set; }
        public Contract()
        {
            this.ContractType = ContractType.Actual;
        }
        public ContractTripProExtension TripProExt { get; set; }
        public ContractMystiflyExtension MystiflyExt { get; set; }

        public AmaduesSelfServiceExtension AmaduesSelfServiceExtension { get; set; }

        //only Use client App
        public int GetStopType()
        {
            int response = 0;
            if (MaxStopOutbound == 0 && (MaxStopInbound == 0 || MaxStopInbound == 1))
            {
                response = 0;
            }
            else
            {
                response = 1;
            }
            //switch (MaxNoOfStopsInContract)
            //{
            //    case 0:
            //        response = 0;
            //        break;
            //    case 1:
            //        response = 1;
            //        break;
            //    default:
            //        response = 2;
            //        break;
            //}
            return response;
        }

        /// <summary>
        /// Get Total  pax
        /// </summary>
        /// <returns></returns>
        public float GetTotalPax()
        {
            return (this.Adult + this.Senior + this.Child + this.InfantOnLap + this.InfantOnSeat);

        }
    }
       
    #region Contract Extension
    public class ContractTripProExtension
    {
        public string ItineraryId { get; set; }
    }
    public class ContractMystiflyExtension
    {
        public string BookingKey { get; set; }
        public bool IsPassportMandatory { get; set; }
        public DateTime? TktTimeLimit { get; set; }
    }
    #endregion

    #region AmaduesSelfService Extension class
    public class AmaduesSelfServiceExtension
    {
        public string Source { get; set; }
        public List<FeesExtension> Fees { get; set; }
        public List<AmaduesSelfServiceTravelerPricing> TravelerPricing { get; set; }
    }
    public class FeesExtension
    {
        public string Amount { get; set; }
        public string Type { get; set; }
    }
    public class AmaduesSelfServiceTravelerPricing
    {
        public string travelerId { get; set; }
        public string fareOption { get; set; }
        public string travelerType { get; set; }
        public AmaduesSelfServicePrice price { get; set; }
        public List<AmaduesSelfServiceFareDetailsBySegment> fareDetailsBySegment { get; set; }

    }
    public class AmaduesSelfServicePrice
    {
        public string currency { get; set; }
        public float total { get; set; }
        [JsonProperty("base")]
        public float basePrice { get; set; }
        public List<FeesExtension> fees { get; set; }
        public float grandTotal { get; set; }
    }
    public class AmaduesSelfServiceFareDetailsBySegment
    {
        public string segmentId { get; set; }
        public string cabin { get; set; }
        public string fareBasis { get; set; }
        public string @class { get; set; }
        public AmaduesSelfServiceIncludedCheckedBags includedCheckedBags { get; set; }
        public string brandedFare { get; set; }
    }
    public class AmaduesSelfServiceIncludedCheckedBags
    {
        public int quantity { get; set; }
    }
    #endregion

    public class BillingDetail
    {
        [DataType(DataType.Text)]
        public string CCHolderName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string CardNumber { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string CVVNumber { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public int CardType { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailConfirm { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        [DataType(DataType.Text)]
        public string StateName { get; set; }
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string BillingPhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }
        public bool IsPrimaryCard { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string AreaCode { get; set; }
        public string CountryCode { get; set; }
    }
    public class Traveller
    {
        public int PaxOrderId { get; set; }
        public int PaxType { get; set; }
        public int Title { get; set; }
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        public string MiddleName { get; set; }
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        public int Gender { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int? DOBDay { get; set; }
        public int DOBMonth { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int? DOBYear { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuingCountry { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
    }
    public class TripDetails
    {
        public List<Segments> OutBoundSegment { get; set; }
        public List<Segments> InBoundSegment { get; set; }
    }
    public class Segments
    {
        public int Id { get; set; }
        public bool IsReturn { get; set; }
        public bool IsDepartDateHighlight { get; set; }
        public bool IsOriginHighlight { get; set; }
        public bool IsDestinationHighlight { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public Airline MarketingCarrier { get; set; }
        public Airline OperatingCarrier { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string StopOverTime { get; set; }
        public string OutTerminal { get; set; }
        public string InTerminal { get; set; }
        public string EquipmentType { get; set; }
        public string FlightNumber { get; set; }
        public string CnxType { get; set; }
        public string FareBasis { get; set; }
        public string Class { get; set; }
        public string PrevClass { get; set; }
        //GDS Cabin
        public string Cabin { get; set; }
        
        public CabinType CabinType { get; set; }
        public string Origin { get; set; }
        public string OriginCity { get; set; }
        public string Destination { get; set; }
        public string DestinationCity { get; set; }
        public TimeSpan? FlightDuration { get; set; }
        public string CompanyFranchiseDetails { get; set; }
        public int AvailableSeats { get; set; }
        public int NoOfStops { get; set; }
        public bool IsSoldOut { get; set; }
        public string AirlineLocator { get; set; }
        public string SegmentStatus { get; set; }
        // public List<TechnicalStop> TechnicalStop { get; set; }
        public SegmentTripProExtension SegmentTripProExt { get; set; }
        public SegmentAmeduesSelfServiceExtension SegmentASSExtension { get; set; }
    }

    #region Segment
    public class SegmentTripProExtension
    {
        public string BaggageAllowance { get; set; }
        public string BaggageInfoUrl { get; set; }
    }
    #endregion
    public class SegmentAmeduesSelfServiceExtension
    {
        public string SegmentId { get; set; }
        public string Number { get; set; }
        public ASSExtensionOperating ASSExtensionOperating { get; set; }
        public string CarrierCode { get; set; }
        public string AirCraftCode { get; set; }
        public int BaggageQuantity { get; set; }
    }
    public class ASSExtensionOperating
    {
        public string carrierCode { get; set; }
    }
    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsLowcost { get; set; }
    }
    public class FareDetails
    {
        public int PaxCount { get; set; }
        public TravellerPaxType PaxType { get; set; }
        public string GDSPaxType { get; set; }
        public float ActualBaseFare { get; set; }
        public float BaseFare { get; set; }
        public float Tax { get; set; }
        public float TotalFare { get; set; }
        public float Markup { get; set; }
        public float SupplierFee { get; set; }
        public float Discount { get; set; }
        public bool IsSellInsurance { get; set; }
        public float InsuranceAmount { get; set; }
        public bool IsSellBaggageInsurance { get; set; }
        public float BaggageInsuranceAmount { get; set; }
        public string FareBaseCode { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public bool IsExtendedCancellation { get; set; }
        public float ExtendedCancellationAmount { get; set; }
        public float BookingFee { get; set; }
        public float TotalFareV2
        {
            get
            {
                return ((BaseFare + Markup + SupplierFee + BookingFee + Tax + (IsExtendedCancellation == true ? ExtendedCancellationAmount : 0) + (IsSellInsurance == true ? InsuranceAmount : 0) + (IsSellBaggageInsurance == true ? BaggageInsuranceAmount : 0)) - Discount) * PaxCount;
            }
        }
        public float TotalFarePPax
        {
            get
            {
                return ((BaseFare + Markup + SupplierFee + BookingFee + Tax + (IsExtendedCancellation == true ? ExtendedCancellationAmount : 0) + (IsSellInsurance == true ? InsuranceAmount : 0) + (IsSellBaggageInsurance == true ? BaggageInsuranceAmount : 0)) - Discount);
            }
        }

        public float BaseFarePPax
        {
            get
            {
                return BaseFare + (IsSellInsurance == true ? InsuranceAmount : 0) + (IsExtendedCancellation == true ? ExtendedCancellationAmount : 0) + (IsSellBaggageInsurance == true ? BaggageInsuranceAmount : 0);
            }
        }
        public float TotalBaseFare
        {
            get
            {
                return (BaseFare + (IsSellInsurance == true ? InsuranceAmount : 0) + (IsExtendedCancellation == true ? ExtendedCancellationAmount : 0) + (IsSellBaggageInsurance == true ? BaggageInsuranceAmount : 0)) * PaxCount;
            }
        }
        public float TaxPPax
        {
            get
            {
                return ((Tax + Markup + SupplierFee + BookingFee));
            }
        }
        public float TotalTax
        {
            get
            {
                return ((Tax + Markup + SupplierFee + BookingFee)) * PaxCount;
            }
        }
    }
}