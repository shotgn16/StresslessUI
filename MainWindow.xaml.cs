using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StresslessUI.DataModels;
using StresslessUI.Http.Methods;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private IHttpClientMethods _httpMethods;
        private IServiceProvider _serviceProvider;
        private ILogger<MainWindow> _logger;

        public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider, IHttpClientMethods httpClientMethods)
        {
            _logger = logger;
            _httpMethods = httpClientMethods;
            _serviceProvider = serviceProvider;

            checkConfiguration();
            InitializeComponent();

            _logger.LogInformation($"MainWindow Loaded! | App Version: 2 | Date: {DateOnly.FromDateTime(DateTime.Now)}");
        }
        private async Task checkConfiguration()
        {
            await _httpMethods.Authorize();
            ConfigurationModel _localConfiguration = await _httpMethods.GetConfiguration();

            try
            {
                if (_localConfiguration != null)
                {
                    var loggedUser = _serviceProvider.GetService<LoggedInForm>();
                    this.Hide();

                    _logger.LogInformation("Class: 'MainWindow' | Function: 'CheckConfiguration' Window Status: ", loggedUser.WindowState);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsPage = _serviceProvider.GetService<SettingsForm>();
            settingsPage.Show();

            _logger.LogInformation("Class: 'MainWindow' | Function: 'OpenSettings' Window Status: ", settingsPage.WindowState);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var registrationPage = _serviceProvider.GetService<RegistrationForm>();
            registrationPage.Show();

            _logger.LogInformation("Class: 'MainWindow' | Function: 'OpenRegistration' Window Status: ", registrationPage.WindowState);
        }

        public void Show()
        {
            try
            {
                this.ShowDialog();
                _logger.LogInformation("Class: 'MainWindow' | Function: 'Show' Status: Success");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}