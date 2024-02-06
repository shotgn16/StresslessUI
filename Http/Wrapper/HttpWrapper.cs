using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using StresslessUI.DataModels;
using StresslessUI.Http.ErrorHandler;

namespace StresslessUI.Http.Wrapper
{
    public class HttpWrapper : Controller, IDisposable, IHttpWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        private readonly ILogger<HttpWrapper> _logger;
        private readonly IHttpError _httpError;

        public HttpWrapper(IHttpClientFactory httpClientFactory, ILogger<HttpWrapper> logger, IHttpError httpError)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _httpError = httpError;

            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task SetAuthorizationHeader(string header, string value)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header, OAuthTokenModel._model.token);
        }

        public async Task<(string, HttpStatusCode)> GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            (string, HttpStatusCode) Return;

            if (!response.IsSuccessStatusCode)
            {
                Return.Item1 = await _httpError.FormatError(response);
                Return.Item2 = response.StatusCode;
            }

            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }

        public async Task<(string, HttpStatusCode)> PostAsync(string url, HttpContent content)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            (string, HttpStatusCode) Return;

            if (!response.IsSuccessStatusCode)
            {
                Return.Item1 = await _httpError.FormatError(response);
                Return.Item2 = response.StatusCode;
            }

            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }

        public async Task<(string, HttpStatusCode)> DeleteAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            (string, HttpStatusCode) Return;

            if (!response.IsSuccessStatusCode)
            {
                Return.Item1 = await _httpError.FormatError(response);
                Return.Item2 = response.StatusCode;
            }

            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }

        public void Dispose() => GC.Collect();
    }
}
