using System.Windows;
using System.Windows.Controls;

namespace StresslessUI.ServiceProvider
{
    public class NavigationService : INaviationService
    {
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void NavigateTo<TWindow>() where TWindow : Window
        {
            var page = ContainerProvider.Resolve<TWindow>();
            _mainFrame.Navigate(page);
        }
    }
}
