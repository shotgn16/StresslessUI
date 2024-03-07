using System.Net.Http;
using System.Net;
using System.Text;
using StresslessUI.DataModels;
using Newtonsoft.Json;
using StresslessUI.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using StresslessUI.Http.Wrapper;

namespace StresslessUI.Http.Methods
{
    public class httpMethods : Controller, IHttpClientMethods, IDisposable
    {
        private ILogger<httpMethods> _logger;
        private IHttpWrapper _httpWrapper;
        public httpMethods(ILogger<httpMethods> logger, IHttpWrapper httpWrapper)
        {
            _httpWrapper = httpWrapper;
            _logger = logger;
        }

        public async Task Authorize()
        {
            if (string.IsNullOrEmpty(OAuthTokenModel._model.token))
            {
                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/Data/Authorize",
                new StringContent(JsonConvert.SerializeObject(new AuthorizationModel
                {
                    MACAddress = await Networks.getMAC(),
                    ClientID = AppSettings.Default.ID
                }),
                Encoding.UTF8, "application/json"));

                _logger.LogInformation("Class: 'HttpMethods' | Function: 'Authorizee' | Status - ", response.Item2 + "\n" + response.Item1);
                OAuthTokenModel._model = JsonConvert.DeserializeObject<OAuthTokenModel>(response.Item1);
            }
        }

        public async Task InsertConfiguration()
        {
            if (!string.IsNullOrEmpty(OAuthTokenModel._model.token))
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/Data/InsertConfiguration",
                    new StringContent(JsonConvert.SerializeObject(ConfigurationModel._model), Encoding.UTF8, "application/json"));

                if (response.Item2 != HttpStatusCode.OK)
                {
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }

                _logger.LogInformation("Class: 'HttpMethods' | Function: 'InsertConfiguration' | Status - ", response.Item2 + "\n" + response.Item1);
            }
        }

        public async Task<ConfigurationModel> GetConfiguration()
        {
            ConfigurationModel _config = new ConfigurationModel();

            if (OAuthTokenModel._model != null)
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.GetAsync($"https://{AppSettings.Default.HostAddress}:7257/Data/GetConfiguration");

                if (response.Item2 != HttpStatusCode.OK || string.IsNullOrEmpty(response.Item1))
                {
                    // LOG
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }

                _logger.LogInformation("Class: 'HttpMethods' | Function: 'GetConfiguration' | Status - ", response.Item2 + "\n" + response.Item1);
                _config = JsonConvert.DeserializeObject<ConfigurationModel>(response.Item1);
            }

            return _config;
        }

        public void Dispose() => GC.Collect();


        public async Task<bool> DeleteConfiguration()
        {
            bool Value = true;
            (string, HttpStatusCode) response = new();

            if (OAuthTokenModel._model != null)
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                response = await _httpWrapper.DeleteAsync($"https://{AppSettings.Default.HostAddress}:7257/Data/DeleteConfiguration");

                if (response.Item2 != HttpStatusCode.OK || string.IsNullOrEmpty(response.Item1))
                {
                    Value = false;
                }
            }

            _logger.LogInformation("Class: 'HttpMethods' | Function: 'DeleteConfiguration' | Status - ", response.Item2 + "\n" + response.Item1);
            return Value;
        }

        public async Task<string> PromptReminder(string Value = null)
        {
            if (!string.IsNullOrEmpty(OAuthTokenModel._model.token))
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/Data/PromptReminder",
                    new StringContent(null, Encoding.UTF8, "application/json"));

                Value = response.Item1;

                if (response.Item2 != HttpStatusCode.OK)
                {
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }

                _logger.LogInformation("Class: 'HttpMethods' | Function: 'PromptReminder' | Status - ", response.Item2 + "\n" + response.Item1);
            }

            return Value;
        }

        // NEXT METHOD - GOES HERE!!!
    }

}
