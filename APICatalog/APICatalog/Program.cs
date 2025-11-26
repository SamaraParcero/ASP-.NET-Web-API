
using APICatalog.Context;
using APICatalog.Extensions;
using APICatalog.Filters;
using APICatalog.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=>
                options.JsonSerializerOptions
                .ReferenceHandler= ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMyService, MyService>();
builder.Services.AddScoped<ApiLoggingFilter>();

//Pegar algo de appsettings
/*
var value1 = builder.Configuration["chave1"];
var value2 = builder.Configuration["secao1:chave2"];
*/
var app = builder.Build();
app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/openapi/v1.json", "API"));
    
}

//Middlewares em ordem
app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
//Middle customizados
/*
app.Use(async (context, next) =>
{
    //Adiciona codigo antes do request
    await next();
    //Adiciona código depois do request
});
*/
app.MapControllers();

app.Run();
