using Microsoft.Extensions.Logging;
using StresslessUI.Settings;
using System.Formats.Asn1;
using System.IO;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for Settings_2.xaml
    /// </summary>
    public partial class SettingsForm : Window, ISettings
    {
        private ILogger<SettingsForm> _logger;
        public SettingsForm(ILogger<SettingsForm> logger)
        {
            _logger = logger;
            InitializeComponent();
            
            tbx_hostAddress.Text = AppSettings.Default.HostAddress;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbx_hostAddress.Text))
            {
                AppSettings.Default.HostAddress = tbx_hostAddress.Text;
            }

            AppSettings.Default.Save();
            _logger.LogInformation("Microservice Address Updated! [{0}]", AppSettings.Default.HostAddress);
            this.Hide();
        }

        private async Task readLogfile()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = @".\logging\ServiceLog-" + currentDate + ".txt";

            if (File.Exists(filePath))
            {
                string content = await File.ReadAllTextAsync(filePath);
                string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    tbx_logs.Text += line + Environment.NewLine;
                }
            }
        }

        public void Show()
        {
            this.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            readLogfile();
        }
    }
}
