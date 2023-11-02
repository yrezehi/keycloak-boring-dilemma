using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// keycloack integration
builder.Services.AddKeycloakAuthentication(builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section).Get<KeycloakAuthenticationOptions>()!);

builder.Services.AddAuthorization(options => options.AddPolicy("IsAdmin", builder =>
    {
        builder.RequireRealmRoles("admin");
        builder.RequireResourceRoles("r-admin");
        // TokenValidationParameters.RoleClaimType is overriden
        // by KeycloakRolesClaimsTransformation
        builder.RequireRole("r-admin");
    }))
    .AddKeycloakAuthorization(builder.Configuration.GetSection(KeycloakProtectionClientOptions.Section).Get<KeycloakProtectionClientOptions>()!);

builder.Services.AddKeycloakAdminHttpClient(builder.Configuration.GetSection(KeycloakAdminClientOptions.Section).Get<KeycloakAdminClientOptions>()!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();