using System;
using System.Net.Http;
using System.Threading.Tasks;
using CrewMobile.Common.Models;
using CrewMobileApi.Apis.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CrewMobileApi.Apis
{
    public class CopaAPIs : ICopaAPIs
    {
        #region Attributes
        /// <summary>
        /// URL APIs Copa
        /// </summary>
        private string urlCopa;

        /// <summary>
        /// URL APIs Copa secured
        /// </summary>
        private string urlCopaSecured;

        /// <summary>
        /// Subscription key to get information from API
        /// </summary>
        private string subscriptionKey;

        /// <summary>
        /// Subscription checkin value to get information from API
        /// </summary>
        private string subscriptionCheckinValue;

        /// <summary>
        /// Subscription irregular operations value to get information from API
        /// </summary>
        private string subscriptionIrregularOperations;

        /// <summary>
        /// Subscription flight operations value
        /// </summary>
        private string subscriptionFlightOperationsValue;

        /// <summary>
        /// Subscription flight operations value
        /// </summary>
        private string subscriptionCheckinSeatsValue;

        /// <summary>
        /// Channel ID Key to consume Graph
        /// </summary>
        private string channelIDKey;

        /// <summary>
        /// Channel ID value to consume Graph
        /// </summary>
        private string channelIDValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public CopaAPIs(IConfiguration configuration)
        {
            //TODO: Change to use configuration manager
            urlCopa = configuration["CopaApi:ApiUrl"];
            urlCopaSecured = configuration["CopaApi:URLCopaSecured"];
            subscriptionKey = configuration["CopaApi:SubscriptionKeyHeader"];
            subscriptionCheckinValue = configuration["RestApiSuscriptionKeys:Checkin"];
            subscriptionFlightOperationsValue = configuration["RestApiSuscriptionKeys:FlightOperations"];
            subscriptionCheckinSeatsValue = configuration["RestApiSuscriptionKeys:CheckinSeats"];
            subscriptionIrregularOperations = configuration["RestApiSuscriptionKeys:IrregularOperations"];
            channelIDKey = configuration["CopaApi:ChannelIDKey"];
            channelIDValue = configuration["CopaApi:ChannelIDValue"];
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get SSR list
        /// </summary>
        /// <param name="flightNumber">Flight number</param>
        /// <param name="departureDate">Departure date</param>
        /// <param name="operatorCarrier">Operator carrier</param>
        /// <param name="origin">Flight origin</param>
        /// <param name="destination">Flight destination</param>
        /// <returns></returns>
        public async Task<Response> GetSSR(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/passengerlist/v1.2/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}?SSRIndicator=true",
                    flightNumber,
                    departureDate,
                    operatorCarrier,
                    origin,
                    destination);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<SSRCom>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Get preferred list
        /// </summary>
        /// <param name="flightNumber">Flight number</param>
        /// <param name="departureDate">Departure date</param>
        /// <param name="operatorCarrier">Operator carrier</param>
        /// <param name="origin">Flight origin</param>
        /// <param name="destination">Flight destination</param>
        /// <returns></returns>
        public async Task<Response> GetPrefer(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/passengerlist/v1.2/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}?PreferPassengerIndicator=true",
                    flightNumber,
                    departureDate,
                    operatorCarrier,
                    origin,
                    destination);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PassengerListHeaderCom>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        //TODO: Validar el uso del parametro de la cabina
        public async Task<Response> GetPassengerList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination,
            string cabinClass)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/passengerlist/v1.2/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}?CabinClass={5}",
                    flightNumber,
                    departureDate,
                    operatorCarrier,
                    origin,
                    destination,
                    cabinClass);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PassengerListHeaderCom>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        //TODO: Validar la reactivacion o reemplazo de la obtencion de los totales
        public async Task<Response> GetCounts(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/passengerlist/v1.2/FlightCount/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}",
                    flightNumber,
                    departureDate,
                    "CM",
                    origin,
                    destination);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<FlightCountHeaderModel>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetPassengerListStandbyList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/passengerlist/v1.2/standbylist/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}",
                    flightNumber,
                    departureDate,
                    operatorCarrier,
                    origin,
                    destination);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<StandbyListHeaderCom>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetApiFlightInformation(
            DateTime departureFrom,
            DateTime departureTo,
            string flightNumber)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionFlightOperationsValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/FlightOperations/v2/Flights/flight?dateofdepartureFrom={0:yyyyMMdd}&dateofdepartureTo={1:yyyyMMdd}&flightnumber={2}",
                    departureFrom,
                    departureTo,
                    flightNumber);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<FlightDetailHeaderCom>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetSeats(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopaSecured);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionCheckinSeatsValue);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var url = string.Format(
                    "/checkin/seats/v1.1/seat/map/{0}/{1:yyyy-MM-dd}/{2}/{3}/{4}",
                    flightNumber,
                    departureDate,
                    "CM",
                    origin,
                    destination);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorApiCom>(errorString);
                    return new Response
                    {
                        IsSuccess = false,
                        Result = error,
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<SeatsHeaderModel>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetIrregularOperations()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlCopa);
                client.DefaultRequestHeaders.Add(subscriptionKey, subscriptionIrregularOperations);
                client.DefaultRequestHeaders.Add(channelIDKey, channelIDValue);
                var date = $"{DateTime.Today:yyyy-MM-dd}";
                var url = $"/Irrops/v1.1/Irrops/FlightDateFrom/FlightDateTo?FlightDateFrom={date}&FlightDateTo={date}&TypeOfIrrop=ALL";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Result = "No irregular operations found.",
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<FlightIrropHeader>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        #endregion
    }
}