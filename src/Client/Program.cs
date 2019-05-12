using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var token = GetToken("http://localhost:5000").GetAwaiter().GetResult();
            Console.WriteLine(token.Json);
            RunClient(token.AccessToken, "http://localhost:5001/identity").GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task<TokenResponse> GetToken(string authAddress)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(authAddress);

            if (disco.IsError)
            {
                throw new Exception(disco.Error); ;
                
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return tokenResponse;
        }

        private static async Task RunClient(string accessToken, string reqAddress)
        {
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync(reqAddress);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.RequestMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
