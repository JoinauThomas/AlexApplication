using AlexClient.Models;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlexClient.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetClaimsAsync(TokenResponse token);
        Task<TokenResponse> GetTokenAsync(LoginModel login);
        TokenResponse GetToken();
        Task<TokenResponse> RefreshTokenAsync(HttpClient client, TokenResponse tokenResponse);
    }
}