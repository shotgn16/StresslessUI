﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace StresslessUI.Http
{
    public class HttpError : Controller
    {
        private ILogger<HttpError> logger;

        public HttpError(ILogger<HttpError> logger)
        {
            this.logger = logger;
        }

        public async Task<string> FormatError(HttpResponseMessage response)
        {
            return "Error " + response.StatusCode + ": " + await response.Content.ReadAsStringAsync();
        }
    }
}
