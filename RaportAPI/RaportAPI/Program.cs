using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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


// Adding Authentication
// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromSeconds(30),

        ValidAudience = config["JwtSettings:Audience"],
        ValidIssuer = config["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Secret"]))
    };
});


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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Name = "Bearer",
                       In = ParameterLocation.Header,
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
});



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

