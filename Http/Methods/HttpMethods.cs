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
                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/Authorize",
                new StringContent(JsonConvert.SerializeObject(new AuthorizationModel
                {
                    MACAddress = await Networks.getMAC(),
                    ClientID = AppSettings.Default.ID
                }),
                Encoding.UTF8, "application/json"));

                OAuthTokenModel._model = JsonConvert.DeserializeObject<OAuthTokenModel>(response.Item1);
            }
        }

        public async Task InsertConfiguration()
        {
            if (!string.IsNullOrEmpty(OAuthTokenModel._model.token))
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/InsertConfiguration",
                    new StringContent(JsonConvert.SerializeObject(ConfigurationModel._model), Encoding.UTF8, "application/json"));

                if (response.Item2 != HttpStatusCode.OK)
                {
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }
            }
        }

        public async Task<ConfigurationModel> GetConfiguration()
        {
            ConfigurationModel _config = new ConfigurationModel();

            if (OAuthTokenModel._model != null)
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.GetAsync($"https://{AppSettings.Default.HostAddress}:7257/GetConfiguration");

                if (response.Item2 != HttpStatusCode.OK || string.IsNullOrEmpty(response.Item1))
                {
                    // LOG
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }

                _config = JsonConvert.DeserializeObject<ConfigurationModel>(response.Item1);
            }

            return _config;
        }

        public void Dispose() => GC.Collect();


        public async Task<bool> DeleteConfiguration()
        {
            bool Value = true;

            if (OAuthTokenModel._model != null)
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.DeleteAsync($"https://{AppSettings.Default.HostAddress}:7257/DeleteConfiguration");

                if (response.Item2 != HttpStatusCode.OK || string.IsNullOrEmpty(response.Item1))
                {
                    Value = false;
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }
            }

            return Value;
        }

        public async Task<bool> PromptReminder(bool Value = false)
        {
            if (!string.IsNullOrEmpty(OAuthTokenModel._model.token))
            {
                await _httpWrapper.SetAuthorizationHeader("Bearer", OAuthTokenModel._model.token);

                (string, HttpStatusCode) response = await _httpWrapper.PostAsync($"https://{AppSettings.Default.HostAddress}:7257/PromptReminder",
                    new StringContent(null, Encoding.UTF8, "application/json"));

                Value = Convert.ToBoolean(response.Item1);

                if (response.Item2 != HttpStatusCode.OK)
                {
                    _logger.LogError(response.Item2 + "\n" + response.Item1);
                }
            }

            return Value;
        }

        // NEXT METHOD - GOES HERE!!!
    }

}
