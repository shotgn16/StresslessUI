using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for Notifi.xaml
    /// </summary>
    public partial class Notifi : Window
    {
        public Notifi(string type, string message)
        {
            InitializeComponent();
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    Image image = new Image();

            switch (type)
            {
                case "SUCCESS":
                    msg_title.Content = type; ;
                        msg_text.Content = message;
                    msg_color1.Fill = new SolidColorBrush(Colors.Green);
                        msg_logo.Source = image.Source = (new ImageSourceConverter()).ConvertFromString("./resources/icons8-ok-48.png") as ImageSource;
                    break;
                case "ERROR":
                    msg_title.Content = type; ;
                        msg_text.Content = message;
                    msg_color1.Fill = new SolidColorBrush(Colors.Red);
                        msg_logo.Source = image.Source = (new ImageSourceConverter()).ConvertFromString("./resources/icons8-cancel-48.png") as ImageSource;
                        break;
                case "INFO":
                    msg_title.Content = type; ;
                        msg_text.Content = message;
                    msg_color1.Fill = new SolidColorBrush(Colors.Blue);
                        msg_logo.Source = image.Source = (new ImageSourceConverter()).ConvertFromString("./resources/icons8-info-48.png") as ImageSource;
                    break;
                case "WARNING":
                    msg_title.Content = type; ;
                        msg_text.Content = message;
                    msg_color1.Fill = new SolidColorBrush(Colors.Yellow);
                        msg_logo.Source = image.Source = (new ImageSourceConverter()).ConvertFromString("./resources/icons8-error-48.png") as ImageSource;
                    break;
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
                this.Close();
        }
    }
}
