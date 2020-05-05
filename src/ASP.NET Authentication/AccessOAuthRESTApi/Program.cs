using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessOAuthRESTApi
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate Authorize Access Token to authenticate REST Web API.  
            var oAuthInfo = GetAuthorizeToken().Result;
            Console.WriteLine(oAuthInfo);

            // Process response access token info.  
            dynamic accessToken = JObject.Parse(oAuthInfo);
            // Call REST Web API method with authorize access token.  
            var apiResult = GetInfo(Convert.ToString(accessToken.access_token)).Result;
            Console.WriteLine(apiResult);

            // Process Result.

            Console.Read();
        }

        public static async Task<string> GetAuthorizeToken()
        {
            // Initialization.  
            string result = string.Empty;

            // Posting.  
            using (var client = new System.Net.Http.HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44340/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                var response = new System.Net.Http.HttpResponseMessage();
                var allIputParams = new List<KeyValuePair<string, string>>();

                // Convert Request Params to Key Value Pair.  
                allIputParams.Add(new KeyValuePair<string, string>("grant_type", "password")); 
                allIputParams.Add(new KeyValuePair<string, string>("username", "admin"));
                allIputParams.Add(new KeyValuePair<string, string>("password", "adminPass"));

                // URL Request parameters.  
                var requestParams = new System.Net.Http.FormUrlEncodedContent(allIputParams);

                // HTTP POST  
                response = await client.PostAsync("Token", requestParams).ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }

            return result;
        }

        public static async Task<string> GetInfo(string authorizeToken)
        {
            // Initialization.  
            string result = string.Empty;

            // HTTP GET.  
            using (var client = new System.Net.Http.HttpClient())
            {
                // Initialization  
                string authorization = authorizeToken;

                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44340/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                var response = new System.Net.Http.HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("api/WebApi").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }

            return result;
        }
    }
}
