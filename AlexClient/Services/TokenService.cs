using AlexClient.Interfaces;
using AlexClient.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlexClient.Services
{
    public class TokenService : ITokenService
    {
        private DiscoveryDocumentResponse disco = new DiscoveryDocumentResponse();
        public TokenResponse tokenResponse { get; private set; }

        public async Task<TokenResponse> GetTokenAsync(LoginModel login)
        {
            var client = new HttpClient();
            tokenResponse = await GetTokenAsync(client, login);

            return tokenResponse;
        }
        public TokenResponse GetToken()
        {
            return tokenResponse;
        }
        public async Task<TokenResponse> RefreshTokenAsync(HttpClient client, TokenResponse tokenResponse)
        {
            tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                RefreshToken = tokenResponse.RefreshToken
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }
            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }
        public async Task<string> GetClaimsAsync(TokenResponse token)
        {
            var client = new HttpClient();
            var claims = await GetClaimsAsync(token, client);

            return claims;
        }


        private async Task<TokenResponse> GetTokenAsync(HttpClient client, LoginModel login)
        {
            disco = await GetDiscoveryDocumentAsync(client);
            var token = await GetTokenResponseAsync(client, login);

            return token;
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
        private async Task<TokenResponse> GetTokenResponseAsync(HttpClient client, LoginModel login)
        {
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                UserName = login.Email,
                Password = login.Password,

                Scope = "api1 offline_access"
            });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }
            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }
        private async Task<string> GetClaimsAsync(TokenResponse token, HttpClient client)
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
