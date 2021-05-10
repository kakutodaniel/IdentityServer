using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var permissions = new[] { "1:10", "1:11", "1:12", "1:13", "1:14", "1:15", "1:16", "1:17", "1:18", "1:19", "1:20", "2:10", "3:10", "4:10",
            "5:10","6:10","7:10","8:10","9:10" };

            client.DefaultRequestHeaders.Add("permission", string.Join(";", permissions));

            var disco = client.GetDiscoveryDocumentAsync("https://localhost:5001").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "client_credentials",
                ClientId = "wl_client",
                ClientSecret = "wl_secret",
                Scope = "email_access promotion_access",
                
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync("https://localhost:6001/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
