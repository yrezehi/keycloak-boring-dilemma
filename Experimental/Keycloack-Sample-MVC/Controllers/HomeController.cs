using Keycloack_Sample_MVC.Models;
using Keycloack_Sample_MVC.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Keycloack_Sample_MVC.Controllers
{
    public class HomeController : Controller
    {
   

        public async Task<ActionResult> Callback()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var refreshToken = HttpContext.Request.Headers["RefreshToken"];
            var decodedToken = DecodeToken(accessToken);
            var username = decodedToken.Claims.FirstOrDefault(x => x.Type == "preferred_username").Value;
            var fullname = decodedToken.Claims.FirstOrDefault(x => x.Type == "name").Value;
            var claims = new[]
            {
                new Claim("UserName",username),
                new Claim("FullName",fullname),
                new Claim("AccessToken",accessToken.ToString()),
                new Claim("RefreshToken",refreshToken.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "keycloak_sso_auth");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { });
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync();
            //Find claims for the current user
            ClaimsPrincipal currentUser = this.User;
            //Get username, for keycloak you need to regex this to get the clean username
            var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //logs an error so it's easier to find - thanks debug.
            _logger.LogError(currentUserName);

            /*
             * Token exchange implementation
             * Uncomment section below
             */
            
            //Call a token exchange to call another service in keycloak
            //Remember to implement a logger with the default constructor for more visibility
            TokenExchangeUtil exchange = new TokenExchangeUtil();
            //Do a refresh token, if the service you need to call has a short lived token time
            var newAccessToken = await exchange.GetTokenAsync();
            //Use the access token to call the service that exchanged the token
            //Example:
            // MyService myService = new MyService/();
            //var myService = await myService.GetDataAboutSomethingAsync(serviceAccessToken):

            var decodedToken = DecodeToken(newAccessToken);
            var username = decodedToken.Claims.FirstOrDefault(x => x.Type == "preferred_username").Value;
            var claims = new[]
            {
                new Claim("UserName",username),
                new Claim("AccessToken",newAccessToken.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "keycloak_sso_auth");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { });

            //Get all claims for roles that you have been granted access to 
            IEnumerable<Claim> roleClaims = User.FindAll(ClaimTypes.Role);
            IEnumerable<string> roles = roleClaims.Select(r => r.Value);

            //Another way to display all role claims
            var currentClaims = currentUser.FindAll(ClaimTypes.Role).ToList();
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}