using ApiCatalog_MinimalAPI.ApiEndpoints;
using ApiCatalog_MinimalAPI.AppServicesExtensions;
using ApiCatalog_MinimalAPI.Context;
using ApiCatalog_MinimalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthenticationJwt();

var app = builder.Build();

app.MapAutenticationEndpoints();
app.MapCategoriesEndpoints();
app.MapProductsEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

var enviroment = app.Environment;

app.UseExceptionHandling(enviroment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
