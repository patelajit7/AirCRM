using Common;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Infrastructure.HelpingModel;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Infrastructure;
using Infrastructure.HelpingModels;

namespace Business
{
    public class RESTClient
    {
        public static async Task<Response<BookingDetail>> RetrivePNRDetails(string _referenceNo, ProviderType providerType)
        {
            Response<BookingDetail> responseWrp = null;
            try
            {
                using (var handler = new WebRequestHandler())
                {
                    handler.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    using (var client = new HttpClient(handler))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        client.BaseAddress = new Uri(Utility.Settings.TravelAPI.ApiPath);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Referrer = new Uri(Utility.Settings.TravelAPI.RequestHeaderReferrer);
                        client.DefaultRequestHeaders.Add(Utility.Settings.TravelAPI.AuthoriseToken.Header, Utility.Settings.TravelAPI.AuthoriseToken.Value);
                        client.DefaultRequestHeaders.Add("X-Provider", providerType.ToString());
                        client.Timeout = new TimeSpan(0, 0, Utility.Settings.TravelAPI.SearchRestClientTimeOut);
                        HttpResponseMessage httpResponseMessage = client.GetAsync(string.Format("{0}?pnr={1}",Utility.Settings.TravelAPI.RetrievePNR, _referenceNo)).Result;
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            responseWrp = await httpResponseMessage.Content.ReadAsAsync<Response<BookingDetail>>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("RESTClient.RetrivePNRDetails|Exception:" + ex.ToString());
            }

            return responseWrp;
        }
        public static string CallResetMarkup()
        {
            string responseXML = null;
            try
            {
                using (var handler = new WebRequestHandler())
                {
                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri(Utility.Settings.APISettings.ResetMarkupUrl);
                        client.DefaultRequestHeaders.Referrer = new Uri(Utility.Settings.APISettings.ResetMarkupUrl);
                        client.Timeout = new TimeSpan(0, 0, Utility.Settings.APISettings.RestClientTimeOutInSecond);
                        Task<string> response = client.GetStringAsync(Utility.Settings.APISettings.ResetMarkupAction);
                        response.Wait();
                        responseXML = response.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("EasyPro.Business.RESTClient.CallResetMarkup|Exception: " + ex);
            }
            return responseXML;
        }

        public static async Task<Response<bool>> CancelPNR(string _referenceNo, ProviderType providerType)
        {
            Response<bool> responseWrp = null;
            try
            {
                using (var handler = new WebRequestHandler())
                {
                    handler.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    using (var client = new HttpClient(handler))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        client.BaseAddress = new Uri(Utility.Settings.TravelAPI.ApiPath);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Referrer = new Uri(Utility.Settings.TravelAPI.RequestHeaderReferrer);
                        client.DefaultRequestHeaders.Add(Utility.Settings.TravelAPI.AuthoriseToken.Header, Utility.Settings.TravelAPI.AuthoriseToken.Value);
                        client.DefaultRequestHeaders.Add("X-Provider", providerType.ToString());
                        client.Timeout = new TimeSpan(0, 0, Utility.Settings.TravelAPI.SearchRestClientTimeOut);
                        HttpResponseMessage httpResponseMessage = client.GetAsync(string.Format("{0}?pnr={1}", "api/flight/cancel-pnr", _referenceNo)).Result;
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            responseWrp = await httpResponseMessage.Content.ReadAsAsync<Response<bool>>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error("RESTClient.CancelPNR|Exception:" + ex.ToString());
            }

            return responseWrp;
        }
    }
}
