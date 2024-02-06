using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StresslessUI.DataModels;
using StresslessUI.Http.Methods;
using StresslessUI.Logic;
using StresslessUI.Registration;
using System.IO;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window, IRegistrationForm
    {
        private ILogger<RegistrationForm> _logger;
        private IHttpClientMethods _httpMethods;
        private ICalenderImport _calenderImport;
        public RegistrationForm(ILogger<RegistrationForm> logger, ICalenderImport calenderImport, IHttpClientMethods httpMethods)
        {
            _logger = logger;
            _httpMethods = httpMethods;
            _calenderImport = calenderImport;

            InitializeComponent();
        }

        private async void btn_saveConfiguration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigurationModel._model.FirstName = tbx_firstname.Text;
                ConfigurationModel._model.LastName = tbx_lastname.Text;
                ConfigurationModel._model.WorkingDays = tbx_workingDays.Text.Split(", ");
                ConfigurationModel._model.DayStartTime = TimeOnly.FromDateTime(Convert.ToDateTime(tbx_startTime.Text));
                ConfigurationModel._model.DayEndTime = TimeOnly.FromDateTime(Convert.ToDateTime(tbx_finishTime.Text));
                ConfigurationModel._model.CalenderImport = tbx_iCalendar.Text;
                ConfigurationModel._model.UiLoc = Directory.GetCurrentDirectory();

                List<CalendarModel> events = await _calenderImport.retrieveEvents(tbx_iCalendar.Text);
                ConfigurationModel._model.Calender = events.ToArray();

                await _httpMethods.InsertConfiguration();

                grid_registrationComplete.Visibility = Visibility.Visible;
                    tbx_configurationOutput.Text = JsonConvert.SerializeObject(ConfigurationModel._model);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public void Show()
        {
            this.ShowDialog();
        }
    }
}
