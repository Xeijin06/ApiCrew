using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//using System.Web.Configuration;
using Microsoft.Extensions.Configuration;
using System.Xml;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CrewMobileApi.Apis
{
    public class CopaSoap : ICopaSoap
    {
        #region Attributes
        private string flifoURL;

        private string timeZoneURL;

        private string flifoAuthorization;

        private string flifoAction;

        private string timeZoneAction;

        private string flifoKey;

        private string timeZoneKey;
        #endregion

        #region Constructors
        public CopaSoap(IConfiguration configuration)
        { 
            flifoURL = configuration["Flifo:URL"];
            flifoAuthorization = configuration["Flifo:Authorization"];
            flifoAction = configuration["Flifo:Action"];
            flifoKey = configuration["Flifo:Key"];

            timeZoneURL = configuration["TimeZone:URL"];
            timeZoneAction = configuration["TimeZone:Action"];
            timeZoneKey = configuration["TimeZone:Key"];
        }
        #endregion

        #region Methods
        public async Task<FlightDetailSoapCom2> GetFlightInformation(string flightNumber, string date)
        {
            //TODO: Validar necesidad de codigo viejo
            /*
            string result = string.Empty;
            string urlAddress = flifoURL;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create the web request
            HttpWebRequest request = WebRequest.Create(new Uri(urlAddress)) as HttpWebRequest;
            request.Headers["Authorization"] = flifoAuthorization;
            request.Headers["SOAPAction"] = flifoAction;
            request.Headers["REPLACE"] = flifoKey;

            // Set type to POST
            request.Method = "POST";
            request.ContentType = "text/xml";

            // Create the data we want to send
            StringBuilder data = new StringBuilder();
            string xmlPayLoad = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:flif=\"http://www.eds.com/AirlineSOASchema/Flifo/\" xmlns:air=\"http://www.opentravel.org/OTA/2003/05/AirFlifoRQ\" xmlns:com=\"http://www.opentravel.org/OTA/2003/05/CommonTypes\">" +
                                "   <soapenv:Header/>" +
                                "   <soapenv:Body>" +
                                "      <flif:getFlifo>" +
                                "         <air:OTA_AirFlifoRQ EchoToken=\"New Revenue Accounting\" Version=\"4.0\">" +
                                "            <air:POS>" +
                                "               <com:Source AirlineVendorID=\"CM\"/>" +
                                "            </air:POS>" +
                                "            <air:FlightSegment>" +
                                "               <air:Airline Code=\"CM\"/>" +
                                "               <air:FlightNumber>" + flightNumber + "</air:FlightNumber>" +
                                "               <air:DepartureDate>" + date + "</air:DepartureDate>" +
                                "            </air:FlightSegment>" +
                                "         </air:OTA_AirFlifoRQ>" +
                                "      </flif:getFlifo>" +
                                "   </soapenv:Body>  " +
                                "</soapenv:Envelope>";

            data.Append(xmlPayLoad);

            // Create a byte array of the data we want to send
            byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());
            try
            {
                // Write data to request
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
            }
            catch (WebException wex)
            {
                var httpResponse = wex.Response as HttpWebResponse;
                var ai = new TelemetryClient();
                ai.TrackException(wex);
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                var ai = new TelemetryClient();
                ai.TrackException(ex);
                return null;
            }
            
            // Get response and return it
            XmlDocument xmlResult = new XmlDocument();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }

                xmlResult.LoadXml(result);
            }
            catch (WebException wex)
            {

                var httpResponse = wex.Response as HttpWebResponse;
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            try
            {
                var json = JsonConvert.SerializeXmlNode(xmlResult);
                var flightDetailSoapCom = JsonConvert.DeserializeObject<FlightDetailSoapCom>(json);
                var ns4FlightLegInfo = new List<Ns4FlightLegInfo>();
                ns4FlightLegInfo.Add(flightDetailSoapCom.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo);
                var flightDetailSoapCom2 = new FlightDetailSoapCom2
                {
                    SoapEnvelope = new SoapEnvelope2
                    {
                        SoapBody = new SoapBody2
                        {
                            Ns3GetFlifoResponse = new Ns3GetFlifoResponse2
                            {
                                Ns4OtaAirFlifoRs = new Ns4OtaAirFlifoRs2
                                {
                                    Ns4FlightInfoDetails = new Ns4FlightInfoDetails2
                                    {
                                        FlightNumber = flightDetailSoapCom.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber,
                                        Ns4FlightLegInfo = ns4FlightLegInfo,
                                    },
                                },
                            },
                        },
                    },
                };
                return flightDetailSoapCom2;
            }
            catch
            {
                try
                {
                    var json = JsonConvert.SerializeXmlNode(xmlResult);
                    var flightDetailSoapCom2 = JsonConvert.DeserializeObject<FlightDetailSoapCom2>(json);
                    return flightDetailSoapCom2;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return null;
                }
            }*/

            //string result = string.Empty;
            string urlAddress = flifoURL;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var soapRequest = string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:flif=""http://www.eds.com/AirlineSOASchema/Flifo/"" xmlns:air=""http://www.opentravel.org/OTA/2003/05/AirFlifoRQ"" xmlns:com=""http://www.opentravel.org/OTA/2003/05/CommonTypes"">
                       <soapenv:Header/>
                       <soapenv:Body>
                          <flif:getFlifo>
                             <air:OTA_AirFlifoRQ EchoToken=""New Revenue Accounting"" Version=""4.0"">
                                <air:POS>
                                   <com:Source AirlineVendorID=""CM""/>
                                </air:POS>
                                <air:FlightSegment>
                                   <air:Airline Code=""CM""/>
                                   <air:FlightNumber>{0}</air:FlightNumber>
                                   <air:DepartureDate>{1}</air:DepartureDate>
                                </air:FlightSegment>
                             </air:OTA_AirFlifoRQ>
                          </flif:getFlifo>
                       </soapenv:Body>
                    </soapenv:Envelope>", flightNumber, date);

            var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, urlAddress)
            {
                Content = new StringContent(soapRequest, Encoding.UTF8, "text/xml")
            };

            httpRequest.Headers.Add("SOAPAction", string.Format(@"""{0}""",flifoAction));
            httpRequest.Headers.Add("Authorization", flifoAuthorization);
            httpRequest.Headers.Add("REPLACE", flifoKey);

            XDocument xmlDoc = new XDocument();

            try
            {
                var response = await httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();
                xmlDoc = XDocument.Parse(responseContent);

                //Console.WriteLine(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            // Get response and return it
            try
            {
                var json = JsonConvert.SerializeXNode(xmlDoc);
                var flightDetailSoapCom = JsonConvert.DeserializeObject<FlightDetailSoapCom>(json);
                var ns4FlightLegInfo = new List<Ns4FlightLegInfo>();
                ns4FlightLegInfo.Add(flightDetailSoapCom.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo);
                var flightDetailSoapCom2 = new FlightDetailSoapCom2
                {
                    SoapEnvelope = new SoapEnvelope2
                    {
                        SoapBody = new SoapBody2
                        {
                            Ns3GetFlifoResponse = new Ns3GetFlifoResponse2
                            {
                                Ns4OtaAirFlifoRs = new Ns4OtaAirFlifoRs2
                                {
                                    Ns4FlightInfoDetails = new Ns4FlightInfoDetails2
                                    {
                                        FlightNumber = flightDetailSoapCom.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber,
                                        Ns4FlightLegInfo = ns4FlightLegInfo,
                                    },
                                },
                            },
                        },
                    },
                };
                return flightDetailSoapCom2;
            }
            catch
            {
                try
                {
                    var json = JsonConvert.SerializeXNode(xmlDoc);
                    var flightDetailSoapCom2 = JsonConvert.DeserializeObject<FlightDetailSoapCom2>(json);
                    return flightDetailSoapCom2;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return null;
                }
            }
        }

        public string GetFlightInformation2(string flightNumber, string date)
        {
            string result = string.Empty;
            string urlAddress = this.flifoURL;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create the web request
            HttpWebRequest request = WebRequest.Create(new Uri(urlAddress)) as HttpWebRequest;
            request.Headers["Authorization"] = flifoAuthorization;
            request.Headers["SOAPAction"] = flifoAction;
            request.Headers["REPLACE"] = flifoKey;

            // Set type to POST
            request.Method = "POST";
            request.ContentType = "text/xml";

            // Create the data we want to send
            StringBuilder data = new StringBuilder();
            string xmlPayLoad = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:flif=\"http://www.eds.com/AirlineSOASchema/Flifo/\" xmlns:air=\"http://www.opentravel.org/OTA/2003/05/AirFlifoRQ\" xmlns:com=\"http://www.opentravel.org/OTA/2003/05/CommonTypes\">" +
                                "   <soapenv:Header/>" +
                                "   <soapenv:Body>" +
                                "      <flif:getFlifo>" +
                                "         <air:OTA_AirFlifoRQ EchoToken=\"New Revenue Accounting\" Version=\"4.0\">" +
                                "            <air:POS>" +
                                "               <com:Source AirlineVendorID=\"CM\"/>" +
                                "            </air:POS>" +
                                "            <air:FlightSegment>" +
                                "               <air:Airline Code=\"CM\"/>" +
                                "               <air:FlightNumber>" + flightNumber + "</air:FlightNumber>" +
                                "               <air:DepartureDate>" + date + "</air:DepartureDate>" +
                                "            </air:FlightSegment>" +
                                "         </air:OTA_AirFlifoRQ>" +
                                "      </flif:getFlifo>" +
                                "   </soapenv:Body>  " +
                                "</soapenv:Envelope>";

            data.Append(xmlPayLoad);

            // Create a byte array of the data we want to send
            byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());
            try
            {
                // Write data to request
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
            }
            catch (WebException wex)
            {
                var httpResponse = wex.Response as HttpWebResponse;
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            // Get response and return it
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }

                return result;
            }
            catch (WebException wex)
            {

                var httpResponse = wex.Response as HttpWebResponse;
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        public TimeZoneSoapCom GetListTimeZone(List<Airport> airports)
        {
            string result = string.Empty;
            string urlAddress = timeZoneURL;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create the web request
            HttpWebRequest request = WebRequest.Create(new Uri(urlAddress)) as HttpWebRequest;
            request.Headers["SOAPAction"] = timeZoneAction;
            request.Headers["REPLACE"] = timeZoneKey;

            // Set type to POST
            request.Method = "POST";
            request.ContentType = "text/xml";

            // Create the data we want to send
            StringBuilder data = new StringBuilder();
            string xmlPayLoad = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v01=\"http://copaservices.com/Utilities/TimeZone/Requests/COPA_TimeZoneInformationRQ/V01\" xmlns:v011=\"http://copaservices.com/Utilities/TimeZone/Common/COPA_TimeZoneCommonTypes/V01\">" +
                                "   <soapenv:Header/>" +
                                "   <soapenv:Body>" +
                                "      <v01:COPA_TimeZoneInformationRQ Version=\"1.0\" EchoToken=\"ERP_TimeZoneCollector {758DD629-9D96-441D-B9FD-52C418955D56}\" TimeStamp=\"2015-07-15T08:06:00.000Z\">" +
                                "         <v01:TimeZones>" +
                                "            <v011:DisplayOffsetChange Begin_UTC_TimeInstant=\"2018-08-28T00:00:00.000Z\" End_UTC_TimeInstant=\"2018-08-28T23:59:59.999Z\"/>";

            foreach (var airport in airports)
            {
                xmlPayLoad +=  $"            <v011:Location LocationCode=\"{airport.AirportCode}\" LocationCategoryCode=\"Airport\"/>";
            }

            xmlPayLoad +=       "         </v01:TimeZones>" +
                                "      </v01:COPA_TimeZoneInformationRQ>" +
                                "   </soapenv:Body>" +
                                "</soapenv:Envelope>";
            data.Append(xmlPayLoad);

            // Create a byte array of the data we want to send
            byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());
            try
            {
                // Write data to request
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }
            }
            catch (WebException wex)
            {
                var httpResponse = wex.Response as HttpWebResponse;
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            // Get response and return it
            XmlDocument xmlResult = new XmlDocument();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }

                xmlResult.LoadXml(result);
            }
            catch (WebException wex)
            {

                var httpResponse = wex.Response as HttpWebResponse;
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            try
            {
                var json = JsonConvert.SerializeXmlNode(xmlResult);
                var timeZoneSoapCom = JsonConvert.DeserializeObject<TimeZoneSoapCom>(json);
                return timeZoneSoapCom;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        #endregion
    }
}