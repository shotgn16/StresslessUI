using StresslessUI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StresslessUI.Http
{
    public interface IHttpClientMethods
    {
        Task Authorize();
        Task InsertConfiguration();
        Task<ConfigurationModel> GetConfiguration();
        Task<bool> DeleteConfiguration();
        Task<bool> PromptReminder(bool Value = false);
    }
}
