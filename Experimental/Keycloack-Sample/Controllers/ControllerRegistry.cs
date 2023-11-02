using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keycloack_Sample.Controllers
{
    public static class ControllerRegistry
    {
        public static void RegisterControllers(this WebApplication application)
        {
            application.IndexEndpoint();
            application.ClaimProperty();
        }

        private static void IndexEndpoint(this WebApplication application) =>
           application.MapGet("/", () => Results.Ok("Entry "));

        private static void ClaimPropertyEndpoint(this WebApplication application) =>
            application.MapGet("/", (ClaimsPrincipal principal) => Results.Ok(principal.Identity!.Name)).RequireAuthorization("Admin");
    }
}
