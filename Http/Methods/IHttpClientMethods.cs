using StresslessUI.DataModels;

namespace StresslessUI.Http.Methods
{
    public interface IHttpClientMethods
    {
        Task Authorize();
        Task InsertConfiguration();
        Task<ConfigurationModel> GetConfiguration();
        Task<bool> DeleteConfiguration();
        Task<string> PromptReminder(string Value = null);
    }
}
