using CategoriesMVC.Models;
using System.Text;
using System.Text.Json;

namespace CategoriesMVC.Services
{
    public class AuthenticationService : IAuthentication
    {
        private const string apiEndpoint = "/api/autoriza/login";
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private TokenViewModel tokenUser;

        public AuthenticationService(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }
        public async Task<TokenViewModel> AuthenticateUser(UserViewModel userVM)
        {
            var client = _clientFactory.CreateClient("AuthenticationApi");
            var user = JsonSerializer.Serialize(userVM);
            StringContent content = new StringContent(user, Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tokenUser = await JsonSerializer.DeserializeAsync<TokenViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return tokenUser ;
        }
    }
}
