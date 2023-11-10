using Keycloack_Sample_MVC.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Keycloack_Sample_MVC.Util
{
    public class TokenExchangeUtil
    {
        static readonly HttpClient client = new HttpClient();

        public TokenExchangeUtil()
        {
        }

        public async Task<string> GetTokenAsync()
        {
            /*
             * Get refresh token
             * Uses the settings injected from startup to read the configuration
             */
            try
            {
                string url = ConfigurationUtil.GetValue<string>("Keycloak:TokenExchange");
                //Important the grant type fro refresh token, must be set to this!
                string grant_type = "password";
                string client_id = ConfigurationUtil.GetValue<string>("Keycloak:ClientId");

                var form = new Dictionary<string, string>
                {
                    {"grant_type", grant_type},
                    {"client_id", "test-client"},
                    {"username", "test@test.com"},
                    {"password", "password"},
                    {"client_secret", "Tgx4lvbyhho7oNFmiIupDRVA8ioQY7PW"}
                };

                HttpResponseMessage tokenResponse = await client.PostAsync(url, new FormUrlEncodedContent(form));
                var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
                JWTToken tok = JsonConvert.DeserializeObject<JWTToken>(jsonContent);
                return tok.AccessToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static JwtSecurityToken DecodeToken(string token) =>
            new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken ?? throw new ArgumentException("Not valid token!");
    }
}
