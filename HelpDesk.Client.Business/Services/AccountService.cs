using HelpDesk.Client.Business.Settings;
using HelpDesk.Client.Business.ViewModels;
using HelpDesk.Client.Dto.Documents;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HelpDesk.Client.Business.Services
{
    public class AccountService : IAccountService, IDisposable
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IOptions<HelpDeskApiSettings> _helpDeskApiSettings;
        private readonly HttpClient _httpClient;
        private readonly string _basicAuthHeaderValue;
        private bool _disposed;

        public AccountService(ILogger<AccountService> logger, IOptions<HelpDeskApiSettings> helpDeskApiSettings)
        {
            try
            {
                _logger = logger;
                _helpDeskApiSettings = helpDeskApiSettings;

                // Configure a single HttpClient instance for this service instance
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_helpDeskApiSettings.Value.WebApiUrl + "Account/"),
                    Timeout = TimeSpan.FromMinutes(10) // preserve longer timeout used previously in Create
                };
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _basicAuthHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                    $"{_helpDeskApiSettings.Value.WebApiUser}:{_helpDeskApiSettings.Value.WebApiPassword}"));
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), _basicAuthHeaderValue);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
        }

        public async Task<UserDto> Login(UserLoginDto userLogin)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Post, nameof(Login), userLogin);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<UserDto>(response).ConfigureAwait(false) ?? new UserDto();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return new UserDto();
        }

        public async Task<IdentityResult> Create(UserRegistrationViewModel model)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Post, nameof(Create), model.UserRegistration);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<IdentityResult>(response).ConfigureAwait(false) ?? IdentityResult.Failed();
                }
                else
                {
                    model.HttpClientError = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
                model.HttpClientError = ex.Message;
            }
            return IdentityResult.Failed();
        }

        public async Task<UserDto> FindUserByEmail(string Email)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Get, $"{nameof(FindUserByEmail)}/{Email}");
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<UserDto>(response).ConfigureAwait(false) ?? new UserDto();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return new UserDto();
        }

        public async Task<string> GenerateEmailConfirmationToken(UserDto userDto)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Get, nameof(GenerateEmailConfirmationToken), userDto);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<string>(response).ConfigureAwait(false) ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return string.Empty;
        }

        public async Task<UserRegisteredDto> Register(UserRegistrationDto userRegistration)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Post, nameof(Register), userRegistration);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<UserRegisteredDto>(response).ConfigureAwait(false) ?? new UserRegisteredDto();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return new UserRegisteredDto();
        }

        public async Task<bool> EmailConfirmed(EmailConfirmedDto emailConfirmed)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Get, nameof(EmailConfirmed), emailConfirmed);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<bool>(response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return false;
        }

        public async Task<string> GeneratePasswordResetToken(UserDto userDto)
        {
            try
            {
                using var request = CreateRequest(HttpMethod.Get, nameof(GeneratePasswordResetToken), userDto);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await ReadJsonAsync<string>(response).ConfigureAwait(false) ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                
            }
            return string.Empty;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string relativePath, object? body = null)
        {
            var request = new HttpRequestMessage(method, relativePath);
            if (body is not null)
            {
                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return request;
        }

        private static async Task<T?> ReadJsonAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(json))
                return default;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _httpClient.Dispose();
        }
    }
}
