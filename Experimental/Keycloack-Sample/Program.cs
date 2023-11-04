using Keycloack_Sample.Controllers;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

// keycloack integration
builder.Services.AddKeycloakAuthentication(builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section).Get<KeycloakAuthenticationOptions>()!);

builder.Services.AddAuthorization(options => options.AddPolicy("IsAdmin", builder =>
    {
        builder.RequireRealmRoles("admin");
        builder.RequireResourceRoles("r-admin");
        builder.RequireRole("r-admin");
    }))
    .AddKeycloakAuthorization(builder.Configuration.GetSection(KeycloakProtectionClientOptions.Section).Get<KeycloakProtectionClientOptions>()!);

builder.Services.AddKeycloakAdminHttpClient(builder.Configuration.GetSection(KeycloakAdminClientOptions.Section).Get<KeycloakAdminClientOptions>()!);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();