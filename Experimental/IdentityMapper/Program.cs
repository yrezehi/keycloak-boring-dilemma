using IdentityMapper.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RepositoryContext>(o => o.UseInMemoryDatabase("INMEMEORYDB"));

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.Run();