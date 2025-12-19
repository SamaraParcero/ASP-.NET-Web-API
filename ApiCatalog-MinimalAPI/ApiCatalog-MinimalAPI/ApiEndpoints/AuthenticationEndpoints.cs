using ApiCatalog_MinimalAPI.Models;
using ApiCatalog_MinimalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace ApiCatalog_MinimalAPI.ApiEndpoints
{
    public static class AuthenticationEndpoints
    {

        public static void MapAutenticationEndpoints(this WebApplication app)
        {
            //Endpoints de login
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login Invalido");
                }

                if (userModel.UserName == "samara" && userModel.Password == "samara#123")
                {
                    var tokenString = tokenService.GenerateToken(
                        app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        userModel
                    );

                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login Invalido");
                }
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Authentication");


        }
    }
}
