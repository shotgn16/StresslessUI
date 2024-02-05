using Microsoft.Extensions.Logging;
using StresslessUI.DataModels;
using StresslessUI.Http;
using StresslessUI.Registration;
using StresslessUI.Settings;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;
        private LoggedInForm _loggedInForm;
        private ISettings _settingdForm;
        private IHttpClientMethods _httpMethods;
        private IRegistrationForm _registrationForm;

        public MainWindow() { }
        public MainWindow(ILogger<MainWindow> logger, IRegistrationForm registrationForm, ISettings settingdForm)
        {
            _logger = logger;
            _settingdForm = settingdForm;
            _registrationForm = registrationForm;

            LoginUser();
            InitializeComponent();
        }

        private async Task LoginUser()
        {
            bool configExists = await retrieveConfig();

            try
            {
                if (configExists == true)
                {
                    _loggedInForm.Show();
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

        public async Task InjectRegistration(RegistrationForm registrationForm)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _registrationForm.Show();
        }
    }
}