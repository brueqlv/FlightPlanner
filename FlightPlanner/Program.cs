using FlightPlanner.Handlers;
using FlightPlanner.Interfaces;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IFlightService, InMemoryFlightStorage>();
builder.Services.AddScoped<IFlightService, DatabaseFlightStorage>();

builder.Services.AddDbContext<FlightDbContext>(option =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    option.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

    option.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
