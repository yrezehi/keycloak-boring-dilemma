using System.Security.Claims;

namespace Keycloack_Sample.Controllers
{
    public static class ControllerRegistry
    {
        public static void RegisterControllers(this WebApplication application) =>
            application.IndexEndpoint();

        private static void IndexEndpoint(this WebApplication application) =>
            application.MapGet("/", (ClaimsPrincipal principal) => 
                Results.Ok(principal.Identity!.Name)
            );
    }
}
