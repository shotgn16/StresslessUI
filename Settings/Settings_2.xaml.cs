using Microsoft.Extensions.Logging;
using StresslessUI.Settings;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for Settings_2.xaml
    /// </summary>
    public partial class Settings_2 : Window, ISettings
    {
        private ILogger _logger;
        public Settings_2(ILogger<Settings_2> logger)
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
        }

        public void Show()
        {
            this.Show();
        }
    }
}
