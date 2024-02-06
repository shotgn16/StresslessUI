using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StresslessUI.Http.ErrorHandler
{
    public interface IHttpError
    {
        Task<string> FormatError(HttpResponseMessage response);
    }
}
