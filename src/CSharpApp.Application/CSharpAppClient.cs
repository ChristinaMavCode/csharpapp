using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application
{
    public class CSharpAppClient
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly HttpClientSettings _httpClientSettings;

        public CSharpAppClient(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, IOptions<HttpClientSettings> httpClientSettings)
        {
            _restApiSettings = restApiSettings.Value;
            _httpClientSettings = httpClientSettings.Value;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_restApiSettings.BaseUrl!);
        }

        public async Task<string> PostDataAsync(string json, string remoteUrl)
        {
            var content = string.IsNullOrWhiteSpace(json) ? null : new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{remoteUrl}"),
                Content = content
            };

            var httpResponseMessage = _httpClient.Send(httpRequestMessage);
            StreamReader reader = new StreamReader(httpResponseMessage.Content.ReadAsStream());
            return await reader.ReadToEndAsync();
        }

        public async Task<string> PostDataWithAuthAsync(string json, string remoteUrl)
        {
            var content = string.IsNullOrWhiteSpace(json) ? null : new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{remoteUrl}"),
                Content = content
            };

            var token = await GetJwtTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = _httpClient.Send(httpRequestMessage);
            StreamReader reader = new StreamReader(httpResponseMessage.Content.ReadAsStream());
            return await reader.ReadToEndAsync();
        }

        public async Task<string> GetDataAsync(string? requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetDataWithAuthAsync(string? requestUri)
        {
            var token = await GetJwtTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private string? _cachedToken;
        private DateTime _tokenExpiry;
        private async Task<string> GetJwtTokenAsync()
        {
            // Reuse cached token if not expired
            if (_cachedToken != null && _tokenExpiry > DateTime.UtcNow.AddSeconds(10))
            {
                return _cachedToken;
            }

            var payload = new
            {
                email = _restApiSettings.Username,
                password = _restApiSettings.Password
            };

            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Auth, payload);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            if (json == null || !json.TryGetValue("access_token", out var token))
                throw new Exception("JWT token not returned from API");

            _cachedToken = token;
            _tokenExpiry = DateTime.UtcNow.AddMinutes(_httpClientSettings.LifeTime);
            return token;
        }
    }
}
