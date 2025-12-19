using ApiCatalog_MinimalAPI.Models;

namespace ApiCatalog_MinimalAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, UserModel user);
    }
}
