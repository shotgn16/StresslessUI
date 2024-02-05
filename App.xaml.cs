using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using StresslessUI.Http;
using StresslessUI.Logic;
using StresslessUI.Registration;
using StresslessUI.Settings;
using System.Windows;

namespace StresslessUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddScoped<IRegistrationForm, RegistrationForm>();
                    services.AddScoped<ISettings, Settings_2>();
                    services.AddScoped<ILoggedInForm, LoggedInForm>();

                    services.AddScoped<IHttpWrapper, HttpWrapper>();
                    services.AddScoped<IHttpClientMethods, httpMethods>();
                    services.AddScoped<ICalenderImport, CalendarImport>();

                }).ConfigureLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                    logBuilder.AddNLog();

                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();

            await AppHost!.StartAsync();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
