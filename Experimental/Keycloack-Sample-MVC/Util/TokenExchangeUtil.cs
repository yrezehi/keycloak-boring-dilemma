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
        public readonly string ClientSecert;

        private static string DEFAULT_GRANT_TYPE = "password";

        public TokenExchangeUtil() =>        
            (
                HttpClient,
                ServerURL,
                ClientId,
                ClientSecert
            ) 
                =
            (
                new HttpClient(),
                ConfigurationUtil.GetValue<string>("Keycloak:TokenExchange"),
                ConfigurationUtil.GetValue<string>("Keycloak:ClientId"),
                ConfigurationUtil.GetValue<string>("Keycloak:ClientSecret")
            );

        public async Task<string> GetTokenAsync()
        {
            HttpResponseMessage tokenResponse = await HttpClient.PostAsync(
                ServerURL,
                new FormUrlEncodedContent(BuildAuthenticationForm("test@test.com", "password"))
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

        private Dictionary<string, string> BuildAuthenticationForm(string username, string password) => new()
        {
            {"grant_type", DEFAULT_GRANT_TYPE},
            {"client_id", ClientId },
            {"client_secret", ClientSecert},
            {"username", username},
            {"password", password},
        };
    }
}
