using AutoMapper;
using AvatarTourSystem_BE;
using AvatarTourSystem_BE.JsonPolicies;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Common;
using Services.Interfaces;
using Services.Services;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//WORKFLOW test 4
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       // options.JsonSerializerOptions.MaxDepth = 64;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient();

builder.Services.AddApiWebService(builder);
//config cors


var googleApiKey = builder.Configuration.GetSection("GoogleMaps:ApiKey").Value;
builder.Services.AddSingleton(new GoogleMapsService(googleApiKey));

var SecretKeyZalo = builder.Configuration.GetSection("ZaloAPI:SecretKeyZalo").Value;

builder.Services.AddHttpClient();
builder.Services.AddSingleton(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new ZaloServices(SecretKeyZalo, httpClient);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("app-cors",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination")
            .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
//use cors
app.UseCors("app-cors");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
