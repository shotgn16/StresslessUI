using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;

namespace StresslessUI.Http
{
    public class HttpWrapper : Controller, IHttpWrapper, IDisposable
    {
        private readonly IHttpClientWrapper _client;
        private readonly ILogger<HttpWrapper> _logger;
        private readonly HttpError _httpError;

        public HttpWrapper(IHttpClientWrapper client, ILogger<HttpWrapper> logger, HttpError httpError)
        {
            _client = client;
            _logger = logger;
            _httpError = httpError;
        }

        public async Task SetAuthorizationHeader(string header, string value)
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(value))
                await _client.SetAuthorizationHeader(header, value);
        }

        public async Task<(string, HttpStatusCode)> GetAsync(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
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
            HttpResponseMessage response = await _client.PostAsync(url, content);
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
            HttpResponseMessage response = await _client.DeleteAsync(url);
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
