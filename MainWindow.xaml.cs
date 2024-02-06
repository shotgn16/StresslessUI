using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StresslessUI.DataModels;
using StresslessUI.Http.Methods;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHttpClientMethods _httpMethods;
        private IServiceProvider _serviceProvider;
        private ILogger<MainWindow> _logger;

        public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider, IHttpClientMethods httpClientMethods)
        {
            _logger = logger;
            _httpMethods = httpClientMethods;
            _serviceProvider = serviceProvider;

            InitializeComponent();
            LoginUser();

            _logger.LogInformation($"MainWindow Loaded! | App Version: 2 | Date: {DateOnly.FromDateTime(DateTime.Now)}");
        }
        private async Task LoginUser()
        {
            bool configExists = await retrieveConfig();

            try
            {
                if (configExists == true)
                {

                    this.Hide();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private async Task<bool> retrieveConfig()
        {
            bool Value = false;

            try
            {
                await _httpMethods.Authorize();
                ConfigurationModel _localInstance = await _httpMethods.GetConfiguration();

                if (_localInstance != null)
                {
                    ConfigurationModel._model = _localInstance;
                    Value = true;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Value;
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsPage = _serviceProvider.GetService<SettingsForm>();
            settingsPage.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var registrationPage = _serviceProvider.GetService<RegistrationForm>();
            registrationPage.Show();
        }
    }
}