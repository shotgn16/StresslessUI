using System.Windows;
using System.Windows.Controls;

namespace StresslessUI.ServiceProvider
{
    public interface INaviationService
    {
        void NavigateTo<TWindow>() where TWindow : Window;
    }
}
