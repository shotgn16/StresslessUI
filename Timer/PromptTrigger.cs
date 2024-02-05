using Microsoft.Extensions.Logging;
using StresslessUI.Http;
using System.Printing;
using System.Timers;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace StresslessUI.Timer
{
    internal class PromptTrigger : IDisposable
    {
        public static bool enablePrompts;

        private ILogger<PromptTrigger> _logger;
        private httpMethods _httpMethods;

        private static System.Timers.Timer _timer;
        private static bool isActive;

        public PromptTrigger(ILogger<PromptTrigger> logger, httpMethods httpMethods)
        {
            _logger = logger;
            _httpMethods = httpMethods;
        }

        public async Task startTimer()
        {
            if (enablePrompts && isActive == false)
            {
                _timer = new System.Timers.Timer(300000);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerFinish);
                _timer.Start();

                isActive = true;
            }
        }

        public async void OnTimerFinish(object source, ElapsedEventArgs e)
        {
            _timer.Stop();
            isActive = false;
            bool promptUser = false;

            promptUser = await _httpMethods.PromptReminder();

            if (promptUser)
            {
                MessageBox.Show(
                    "Time to take a 20 minuit break!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            await startTimer();
        }

        public void Dispose() => GC.Collect();
    }
}
