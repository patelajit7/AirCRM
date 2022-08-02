using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Configuration
{
    public class Settings
    {
        [XmlElement]
        public bool EnableBundling { get; set; }
        [XmlElement]
        public string BrandName { get; set; }
        [XmlElement]
        public string DomainUrl { get; set; }
        [XmlElement]
        public string AirlineLogoLocation { get; set; }
        [XmlElement]
        public string ExcelPath { get; set; }
        [XmlElement]
        public string ScheduleTime { get; set; }
        [XmlElement]
        public EmailSettings EmailSettings { get; set; }
        [XmlElement]
        public TravelAPI TravelAPI { get; set; }
        [XmlElement]
        public bool TwoStepVerificationEnable { get; set; }

        [XmlElement]
        public string TwoStepVerificationEmail { get; set; }

        [XmlArray(ElementName = "TwoStepsSkipEmails")]
        [XmlArrayItem("Email")]
        public List<string> TwoStepsSkipEmails { get; set; }

        [XmlArray(ElementName = "Countries")]
        [XmlArrayItem("Country")]
        public List<Country> Country { get; set; }

        [XmlArray(ElementName = "USState")]
        [XmlArrayItem("State")]
        public List<State> USState { get; set; }

        [XmlArray(ElementName = "CanadaState")]
        [XmlArrayItem("State")]
        public List<State> CanadaState { get; set; }

        [XmlElement]
        public APISettings APISettings { get; set; }

    }
    public class EmailSettings
    {
        [XmlElement]
        public string SMTPClient { get; set; }
        [XmlElement]
        public int Port { get; set; }
        [XmlElement]
        public bool EnableSSL { get; set; }
        [XmlElement]
        public string Email { get; set; }
        [XmlElement]
        public string Password { get; set; }

    }
    public class TravelAPI
    {
        [XmlElement]
        public string ApiPath { get; set; }
        [XmlElement]
        public string SearchAction { get; set; }
        [XmlElement]
        public string CheckAvailabilityAction { get; set; }
        [XmlElement]
        public string BookingAction { get; set; }
        [XmlElement]
        public string RetrievePNR { get; set; }
        [XmlElement]
        public string RequestHeaderReferrer { get; set; }
        [XmlElement]
        public AuthoriseToken AuthoriseToken { get; set; }
        [XmlElement]
        public int SearchRestClientTimeOut { get; set; }
        [XmlElement]
        public int CheckAvailRestClientTimeOut { get; set; }
        [XmlElement]
        public int BookingRestClientTimeOut { get; set; }

    }
    public class AuthoriseToken
    {
        [XmlAttribute]
        public string Header { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
    public class Country
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }

    public class State
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }
    public class APISettings
    {
        [XmlElement]
        public string ResetMarkupUrl { get; set; }
        [XmlElement]
        public string ResetMarkupAction { get; set; }
        [XmlElement]
        public int RestClientTimeOutInSecond { get; set; }

    }
}
