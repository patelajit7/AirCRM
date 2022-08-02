#region Using Statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
#endregion
namespace Infrastructure
{
    public enum BookingSearchType : int
    {
        [Description("None")]
        None = 0,
        [Description("PNR")]
        PNR = 1,
        [Description("Booking Number")]
        BookingNumber = 2,
        [Description("Email")]
        Email = 4,
        [Description("First Name")]
        FirstName = 6,
        [Description("Last Name")]
        LastName = 7,
        [Description("Card Holder Name")]
        CardHolderName = 5,
        [Description("Contact Number")]
        ContactNumber = 8
    };
    public enum BookingStatus : int
    {

        [Description("None")]
        None = 0,
        [Description("Pending")]
        Pending = 1,
        [Description("In Progress")]
        InProgress = 2,
        [Description("Completed")]
        Completed = 3,
        [Description("Cancelled")]
        Cancelled = 4,
    }
    public enum TripType : byte
    {
        NONE = 0,
        [Description("One way")]
        ONEWAY = 1,
        [Description("Round trip")]
        ROUNDTRIP = 2,
        [Description("Multi job")]
        MULTICITY = 3
    }
    public enum TravellerPaxType : int
    {
        [Description("Pax Type")]
        None = 0,
        [Description("Adult")]
        ADT = 1,
        [Description("Senior")]
        SEN = 2,
        [Description("Child")]
        CHD = 3,
        [Description("Infant on Seat")]
        INS = 4,
        [Description("Infant on lap")]
        INL = 5
    }
    public enum BookingTimeZone : int
    {
        [Description("Coordinated Universal Time")]
        CoordinatedUniversalTime = 0,
        [Description("Pacific Standard Time")]
        PacificStandardTime = 1,
        [Description("Philippine Time Zone")]
        PhilippineTimeZone = 2,
        [Description("Eastern Standard Time")]
        EasternStandardTime = 3,
        [Description("India Time Zone")]
        IndiaTimeZone = 4
    }
    public enum PaymentMethod : int
    {
        [Description("Payment Card")]
        None = 0,
        [Description("Visa")]
        Visa = 1,
        [Description("Master Card")]
        MasterCard = 2,
        [Description("American Express")]
        AmericanExpress = 3,
        [Description("Diners Club")]
        DinersClub = 4,
        [Description("Discover")]
        Discover = 5,
        [Description("Electron")]
        Electron = 6,
        [Description("Maestro")]
        Maestro = 7,
        [Description("BC Card")]
        BCCard = 8,
        [Description("JCB")]
        JCB = 9,
        [Description("CC")]
        CC = 10

    }
    public enum TravellerTitleType : int
    {
        [Description("Title Type")]
        None = 0,
        [Description("Mr.")]
        MR = 1,
        [Description("Mrs.")]
        MRS = 2,
        [Description("Ms.")]
        MS = 3,

    }
    public enum GenderType : int
    {
        [Description("Select")]
        None = 0,
        [Description("Male")]
        Male = 1,
        [Description("Female")]
        Female = 2
    }
    public enum CabinType : int
    {
        None = 0,
        [Description("Economy")]
        Economy = 1,
        [Description("Economy Coach")]
        EconomyCoach = 2,
        [Description("Premium Economy")]
        PremiumEconomy = 3,
        [Description("Business")]
        Business = 4,
        [Description("Premium Business")]
        PremiumBusiness = 5,
        [Description("First")]
        First = 6,
        [Description("Basic Economy")]
        BasicEconomy = 7,
        [Description("Premium First")]
        PremiumFirst = 8
    }
    public enum OriginType : byte
    {
        None = 0,
        Airport = 1,
        City = 2
    }
    public enum StopType : byte
    {
        [Description("Non Stop")]
        NonStop = 0,
        [Description("One Stop")]
        OneStop = 1,
        Multi_Stop1 = 1 << 2,
        Multi_Stop2 = 1 << 3,
        Multi_Stop3 = 1 << 4,
        Multi_Stop4 = 1 << 5,
        Multi_Stop5 = 1 << 6,
        [Description("Multi Stop")]
        Multi_Stops = Multi_Stop1 | Multi_Stop2 | Multi_Stop3 | Multi_Stop4 | Multi_Stop5
    }
    public enum BookingSourceType : int
    {
        None = 0,
        OnlineBooking = 1,
        OfflineBooking = 2
    }
    public enum ProviderType : int
    {
        NONE = 0,
        AMADEUSSELFSERVICE = 6

    }
    public enum PortalType : int
    {
        [Description("FlightsChoice")]
        FlightsChoice = 1000
    }
    public enum RoleType : int
    {
        Admin = 1,
        Agent = 2,
        Supervisor = 3
    }
    public enum CurrencyType : int
    {
        None = 0,
        USD = 1
    }
    public enum ContractType : int
    {
        None = 0,
        Actual = 1,
        NearBy = 2,
        Flexi = 3,
        NearByFlexi = 4,
        PhoneOnly = 5
    }
    public enum FareType
    {
        PUBLISHED,
        PRIVATE
    }
    public enum MarkupType
    {
        Amount = 1,
        Percentage = 2
    }
    public enum DiscountType
    {
        Amount = 1,
        Percentage = 2
    }
    public enum RouteType
    {
        Domestic = 1,
        International = 2
    }
    public enum GDSType : int
    {
        NONE = 0,
        AMADEUS = 1,
        TRIPPRO = 2,
        MYSTIFLY = 3,
        SABRE = 4
    }
    public enum EmailType : int
    {
        None = 0,
        [Description("Booking Receipt")]
        BookingReceipt = 1,
        [Description("Etickets")]
        ETickets = 2,
        [Description("DocuSign")]
        DocuSign = 3,
        [Description("Payment Receipt")]
        PaymentReceipt = 4,
        [Description("Payment Confirmation")]
        PaymentConfirmation = 5
    }
    public enum BookingSubStatus : int
    {
        [Description("TICKET AND MCO ISSUED")]
        TicketAndMCOIssued = 5,
        [Description("AL BOOKING MCO ISSUED")]
        ALBookingMCOIssued = 10,
        [Description("MCO CHARGED FOR CHANGES & CANCELLATIONS")]
        MCOChargedForChangesNCancellations = 15,
        [Description("MCO CHARGED FOR CHANGES")]
        MCOChargedForChanges = 20,
        [Description("TICKET ISSUED ON SPIRIT")]
        TicketIssuedOnSpirit = 25,
    }
    public enum BagInsuranceType : int
    {
        NONE = 0,
        GOLD = 1,
        PLATINUM = 2,
        DIAMOND = 3
    }
    public enum BookingType : int
    {
        None = 0,
        Flight = 1,
        Hotel = 2,
        Car = 3
    }
    public enum AffiliateType : int
    {
        FlightsChoice = 1000
    }
}
