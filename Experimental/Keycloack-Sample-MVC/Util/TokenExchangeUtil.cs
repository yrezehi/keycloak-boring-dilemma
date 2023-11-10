using Keycloack_Sample_MVC.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Keycloack_Sample_MVC.Util
{
    public class TokenExchangeUtil
    {
        public readonly HttpClient HttpClient;
        public readonly string ServerURL;
        public readonly string ClientId;

        public TokenExchangeUtil() =>        
            (HttpClient, ServerURL, ClientId) 
                =
            (new HttpClient(), ConfigurationUtil.GetValue<string>("Keycloak:TokenExchange"), ConfigurationUtil.GetValue<string>("Keycloak:ClientId"));

        public async Task<string> GetTokenAsync()
        {
            HttpResponseMessage tokenResponse = await HttpClient.PostAsync(
                ServerURL,
                new FormUrlEncodedContent(BuildAuthenticationForm())
            );

            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();

            JWTToken? token = JsonConvert.DeserializeObject<JWTToken?>(jsonContent)!;
            
            if(token != null)
            {
                return token.AccessToken;
            }

            throw new ArgumentException("Invalid or missing token!");
        }

        public static JwtSecurityToken DecodeToken(string token) =>
            new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken ?? throw new ArgumentException("Not valid token!");

        private Dictionary<string, string> BuildAuthenticationForm() => new()
        {
            {"grant_type", "password"},
            {"client_id", ClientId },
            {"username", "test@test.com"},
            {"password", "password"},
            {"client_secret", "Tgx4lvbyhho7oNFmiIupDRVA8ioQY7PW"}
        };
    }
}
