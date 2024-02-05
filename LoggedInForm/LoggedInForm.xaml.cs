using Accessibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StresslessUI.DataModels;
using StresslessUI.Http;
using StresslessUI.Registration;
using StresslessUI.Timer;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for LoggedInForm.xaml
    /// </summary>
    public partial class LoggedInForm : Window, ILoggedInForm, IDisposable
    {
        private ILogger _logger;
        private IRegistrationForm _registrationForm;
        private IHttpClientMethods _httpMethods;
        public LoggedInForm(ILogger logger, IHttpClientMethods httpMethods, IRegistrationForm registrationForm)
        {
            _logger = logger;
            _httpMethods = httpMethods;
            _registrationForm = registrationForm;

            InitializeComponent();

            lbl_firstname.Content = ConfigurationModel._model.FirstName;
                tbx_displayConfiguration.Text = JsonConvert.SerializeObject(ConfigurationModel._model);
            tbx_displayConfiguration.TextWrapping = TextWrapping.Wrap;
        }

        private async void btn_deleteConfiguration_Click(object sender, RoutedEventArgs e)
        {
            bool isDeleted = false;

            isDeleted = await _httpMethods.DeleteConfiguration();

            if (isDeleted)
            {
                MessageBox.Show(
                    "Configuration Successfully Deleted!", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btn_changeConfiguration_Click(object sender, RoutedEventArgs e)
        {
            _registrationForm.Show();
                this.Hide();
        }

        private void btn_enablePrompts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Toggle ON/OFF
                if (PromptTrigger.enablePrompts == true)
                {
                    PromptTrigger.enablePrompts = false;
                }

                else if (PromptTrigger.enablePrompts == false)
                {
                    PromptTrigger.enablePrompts = true;
                }

                switch (PromptTrigger.enablePrompts)
                {
                    case true:
                        MessageBox.Show(
                            "Prompts are enabled!", "Information",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case false:
                        MessageBox.Show(
                            "Prompts are dissabled!", "Information", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public void Dispose() => GC.Collect();
    }
}
