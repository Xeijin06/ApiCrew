using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Graph.Models;

namespace CrewMobileApi.Services
{
    public class GraphService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfidentialClientApplication _clientApp;
        private readonly HttpClient _httpClient;

        public GraphService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            _clientApp = ConfidentialClientApplicationBuilder.Create(_configuration["AzureAd:ClientId"])
                .WithClientSecret(_configuration["AzureAd:ClientSecret"])
                .WithAuthority(new Uri($"{_configuration["AzureAd:Instance"]}{_configuration["AzureAd:TenantId"]}"))
                .Build();
        }

        /* TODO: Revisar si este codigo es necesario.
        public IActionResult SignIn()
        {
            var clientId = _configuration["AzureAd:ClientId"];
            var tenantId = _configuration["AzureAd:TenantId"];
            var redirectUri = _configuration["AzureAd:RedirectUri"];
            var scope = "https://graph.microsoft.com/.default";
            var state = Guid.NewGuid().ToString();

            var authorizationUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize" +
                                   $"?client_id={clientId}" +
                                   $"&response_type=code" +
                                   $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                                   $"&response_mode=query" +
                                   $"&scope={Uri.EscapeDataString(scope)}" +
                                   $"&state={state}";

            return Redirect(authorizationUrl);
        }
        */

        private async Task<string> GetAccessTokenAsync()
        {
            /* Old Code
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            //var scopes = new[] { "REPLACE/.default" };
            var result = await _clientApp.AcquireTokenByAuthorizationCode(scopes, "autorizationcode").ExecuteAsync();
                //.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
            */

            /* New Code
            string clientId = _configuration["AzureAd:ClientId"] ?? string.Empty;
            string clientSecret = _configuration["AzureAd:ClientSecret"] ?? string.Empty;
            string tenantId = _configuration["AzureAd:TenantId"] ?? string.Empty;*/

            string[] scopes = _configuration.GetSection("AzureAd:Scope_graph").Get<string[]>() ?? Array.Empty<string>();

            /*var app = ConfidentialClientApplicationBuilder.Create(clientId)
                        .WithClientSecret(clientSecret)
                        .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                        .Build();*/

            var result = await _clientApp.AcquireTokenForClient(scopes)
                                   .ExecuteAsync();
            return result.AccessToken;
        }

        public async Task<JObject> GetUserInformationByEmailAsync(string email)
        {
            var accessToken = await GetAccessTokenAsync();

            string vGraph = _configuration["AzureAd:graph_version"] ?? string.Empty;

            var requestUrl = $"https://graph.microsoft.com/{vGraph}/users?$filter=mail eq '{email}'";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var user = JObject.Parse(content);

            return user;
        }


        public IDictionary<string, object> GetExtendedProperties(User user)
        {
            if (user == null)
            {
                return null;
            }

            //Todo: Validar necesidad del codigo
            //User usr = user as Microsoft.Azure.ActiveDirectory.GraphClient.User;
            //var extendedProperties = usr.GetExtendedProperties();
            var extendedProperties = user.AdditionalData;

            return extendedProperties;
        }
    }
}
