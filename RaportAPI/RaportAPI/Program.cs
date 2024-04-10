using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaportAPI.Core.Application;
using RaportAPI.Core.Application.Raports.Queries.GetRaports;
using RaportAPI.Core.Common;
using RaportAPI.Infrastructure.Infrastructure;
using RaportAPI.Infrastructure.Persistence;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var AllowAnyOrigins = "AllowAnyOrigins";
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var environment = builder.Environment;

//API Logs
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(config)
  .Enrich.FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

// Add API Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);
builder.Services.AddPersistence(config);

//Cors
builder.Services.AddCors(options =>
        options.AddPolicy(
            name: AllowAnyOrigins,
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
                       .WithOrigins("http://localhost:4200")
                       .AllowCredentials()
                       .WithExposedHeaders("Content-Disposition");
            }));



// Controllers
builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Middlewares
app.ConfigureCustomExceptionMiddleware();

app.UseRouting();

//Cors
app.UseCors(AllowAnyOrigins);

app.MapControllers();

app.Run();

