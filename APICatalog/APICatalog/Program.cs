
using APICatalog.Context;
using APICatalog.Extensions;
using APICatalog.Filters;
using APICatalog.Logging;
using APICatalog.Models;
using APICatalog.RateLimitOptions;
using APICatalog.Repositorys;
using APICatalog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOpenApi(options =>
{
    
    options.AddDocumentTransformer((document, context, ct) =>
    {
 
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();

        document.Components.SecuritySchemes["Bearer"] = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "Authorization",
            Description = "Insira: Bearer {seu token}"
        };

        document.SecurityRequirements.Add(
            new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        );

        return Task.CompletedTask;
    });
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
    
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("InvalidSecretKey!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))

    };

});

builder.Services.AddTransient<IMyService, MyService>();
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

//Pegar algo de appsettings
/*
var value1 = builder.Configuration["chave1"];
var value2 = builder.Configuration["secao1:chave2"];
*/


var OrigenWithAcessAllowed = "OrigensWithAcessAllowed";
builder.Services.AddCors(options =>
{
    /*
    options.AddPolicy(OrigenWithAcessAllowed,
        policy =>
        {
            //Defino origiens
            policy.WithOrigins("https://apirequest.io")
            .WithMethods("GET" ,"POST")
            .AllowAnyHeader()
            .AllowCredentials(); 
        });
    */

    options.AddPolicy("OrigenWithAcessAllowed",
        policy =>
        {
            //Defino origiens
            policy.WithOrigins("https://localhost:7022")
            .WithMethods("GET", "POST")
            .AllowAnyHeader();
        });

    options.AddPolicy("EnableCORS",
        policy =>
        {
            //Defino origiens
            policy.WithOrigins("https://localhost:4200")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

//Defino quem tem permissao para acessar
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SUPERADMIN").RequireClaim("id", "SamaraParcero"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("USER"));
    options.AddPolicy("ExclusiveOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" &&
    claim.Value == "SamaraParcero") || context.User.IsInRole("SUPERADMIN")));
});


var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);


builder.Services.AddRateLimiter(rateLimiteroptions =>
{
    rateLimiteroptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
    {
        options.PermitLimit = 1;
        options.Window = TimeSpan.FromSeconds(5);
        options.QueueLimit = 2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    rateLimiteroptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

//RateLimiting global 
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User?.Identity?.Name
                ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(10)
            }));
});


builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/openapi/v1.json", "API"));
    
}




//Middlewares em ordem - IMPORTANTE
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//Depois do routing
app.UseRateLimiter();
app.UseCors("OrigenWithAcessAllowed");
app.UseAuthentication();
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
