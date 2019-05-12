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
            try
            {
                var token = GetToken("http://localhost:5000").GetAwaiter().GetResult();
                Console.WriteLine(token.Json);
                RunClient(token.AccessToken, "http://localhost:5001/identity").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
           
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

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",
                Scope = "api1",
                
                UserName="alice",
                Password="password"
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
