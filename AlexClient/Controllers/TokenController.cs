using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlexClient.Controllers
{
    public class TokenController : Controller
    {
        private DiscoveryDocumentResponse disco = new DiscoveryDocumentResponse();

        [HttpGet]
        public async Task<string> GetTokenAsync()
        {
            var client = new HttpClient();
            var token = await GetToken(client);
            var refreshToken = await GetRefreshToken(client, token);

            var tokenInJson = JsonConvert.SerializeObject(token);
            var refreshTokenInJson = JsonConvert.SerializeObject(refreshToken);

            string result = $"First Token : \n{tokenInJson}\n\n\nRefresh token :\n{refreshTokenInJson}";

            return result;
        }

        [HttpGet]
        public async Task<string> GetClaimsAsync()
        {
            var client = new HttpClient();
            var token = await GetToken(client);
            var claims = await GetClaims(token, client);

            return claims;
        }

        private async Task<TokenResponse> GetToken(HttpClient client)
        {
            
            disco = await GetDiscoveryDocumentAsync(client);
            var token = await GetTokenResponse(client);

            return token;
        }
        private async Task<TokenResponse> GetRefreshToken(HttpClient client, TokenResponse tokenResponse)
        {
            var tokenResponseRefresh = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                RefreshToken = tokenResponse.RefreshToken
            });

            if (tokenResponseRefresh.IsError)
            {
                Console.WriteLine(tokenResponseRefresh.Error);
                return null;
            }
            Console.WriteLine(tokenResponseRefresh.Json);

            return tokenResponseRefresh;
        }
        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client)
        {
            var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discoveryDoc.IsError)
            {
                Console.WriteLine(discoveryDoc.Error);
                return null;
            }

            return discoveryDoc;
        }
        private async Task<TokenResponse> GetTokenResponse(HttpClient client)
        {
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                UserName = "test",
                Password = "password",

                Scope = "api1 offline_access"
            });
            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }
            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }
        private async Task<string> GetClaims(TokenResponse token, HttpClient client)
        {
            client.SetBearerToken(token.AccessToken);

            var result = await client.GetAsync("https://localhost:5001/api/user/GetClaims");

            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode);
                return $"mauvais résultat : status code = {result.StatusCode} ";
            }
            var content = await result.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));

            return content;
        }
    }
}