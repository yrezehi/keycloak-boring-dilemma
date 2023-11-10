using Keycloack_Sample_MVC.Models;
using Keycloack_Sample_MVC.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Keycloack_Sample_MVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Callback()
        {
            string? accessToken = HttpContext.Request.Headers["Authorization"];

            if(accessToken == null)
            {
                return BadRequest("Authorization wasn't provided!");
            }

            var decodedToken = TokenExchangeUtil.DecodeToken(accessToken);

            var usernameClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "preferred_username");

            if (usernameClaim == null)
            {
                return BadRequest("Username wasn't provided!");
            }

            var nameClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "name");

            if (nameClaim == null)
            {
                return BadRequest("Name wasn't provided!");
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim("UserName",usernameClaim.Value),
                new Claim("Name", nameClaim.Value),
                new Claim("AccessToken",accessToken.ToString()),
            }, "keycloak_sso_auth");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { });
            
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Index() =>
            View();
 
        public IActionResult Privacy() =>
            View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      
    }
}