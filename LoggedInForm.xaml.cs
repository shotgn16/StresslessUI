using Accessibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StresslessUI.DataModels;
using StresslessUI.Http;
using StresslessUI.Timer;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for LoggedInForm.xaml
    /// </summary>
    public partial class LoggedInForm : Window, IDisposable
    {
        private ILogger _logger;
        private httpMethods _httpMethods;

        public LoggedInForm() { }
        public LoggedInForm(ILogger logger, httpMethods httpMethods)
        {
            _logger = logger;

            InitializeComponent();
            _httpMethods = httpMethods;

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
                Notifi n = new Notifi("SUCCESS", "Configuration Successfully Deleted!");
                    n.Show();
            }
        }

        private void btn_changeConfiguration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationForm form = new RegistrationForm(1);
                form.Show();
            this.Hide();
        }

        private void btn_enablePrompts_Click(object sender, RoutedEventArgs e)
        {
            Notifi notification = new(null, null);

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
                        notification = new Notifi("INFO", "Prompts are enabled!");
                        break;
                    case false:
                        notification = new Notifi("INFO", "Prompts are dissabled!");
                        break;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            notification.Show();
        }

        public void Dispose() => GC.Collect();
    }
}
