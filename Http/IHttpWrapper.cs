using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StresslessUI.Http
{
    internal interface IHttpWrapper
    {
        Task SetAuthorizationHeader(string header, string value);
        Task<(string, HttpStatusCode)> GetAsync(string url);
        Task<(string, HttpStatusCode)> PostAsync(string url, HttpContent content);
        Task<(string, HttpStatusCode)> DeleteAsync(string url);
        void Dispose();
    }
}
