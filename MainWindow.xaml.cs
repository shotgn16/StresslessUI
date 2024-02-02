using Microsoft.Extensions.Logging;
using StresslessUI.DataModels;
using StresslessUI.Http;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        private LoggedInForm LoggedInForm;
        private httpMethods _httpMethods;

        public MainWindow() { }

        public MainWindow(ILogger logger, LoggedInForm loggedInForm, httpMethods httpMethods)
        {
            _logger = logger;
            LoginUser();

            InitializeComponent();
            _httpMethods = httpMethods;
            LoggedInForm = loggedInForm;
        }

        private async Task LoginUser()
        {
            bool configExists = await retrieveConfig();

            try
            {
                if (configExists == true)
                {
                    LoggedInForm.Show();
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
            Settings_2 settings = new Settings_2((ILogger<Settings_2>)_logger);
            settings.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrationForm registration = new RegistrationForm();
            registration.Show();
        }
    }
}