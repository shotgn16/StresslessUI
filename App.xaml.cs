using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using StresslessUI.Http.ErrorHandler;
using StresslessUI.Http.Methods;
using StresslessUI.Http.Wrapper;
using StresslessUI.Logic;
using StresslessUI.ServiceProvider;
using System.Windows;
using System.Windows.Controls;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
                appDomain.UnhandledException += new UnhandledExceptionEventHandler(exceptionHandler.OnUnhandledException);

            base.OnStartup(e);
            ConfigureServices();
            ShowHomePage();
        }

        private void ConfigureServices()
        {
            // Register WPF Pages...
            var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<MainWindow>();
                serviceCollection.AddScoped<RegistrationForm>();
                serviceCollection.AddScoped<LoggedInForm>();
                serviceCollection.AddScoped<SettingsForm>();

            // Register NLog logger
            serviceCollection.AddLogging(builder => builder.AddNLog());
            serviceCollection.AddHttpLogging(e =>
            {
                e.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            });

            // Register App Services
            serviceCollection.AddTransient<IHttpClientMethods, httpMethods>();
            serviceCollection.AddTransient<IHttpWrapper, HttpWrapper>();
            serviceCollection.AddTransient<IHttpError, HttpError>();
            serviceCollection.AddTransient<ICalenderImport, CalendarImport>();

            // Register IHttpClientFactory
            serviceCollection.AddHttpClient();

            serviceCollection.AddSingleton<Frame>(provider =>
            {
                var mainWindow = provider.GetRequiredService<MainWindow>();
                return new Frame { Content = mainWindow };
            });

            // Register navigation service
            serviceCollection.
                AddSingleton<INaviationService, NavigationService>();

            // Build the container
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Set the container as the 'Autofac' DI container
            ContainerProvider.SetContainerProvider(_serviceProvider);
        }

        private void ShowHomePage()
        {
            var mainPage = _serviceProvider.GetService<MainWindow>();
            mainPage.ShowDialog();
        }
    }
}
