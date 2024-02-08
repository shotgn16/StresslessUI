using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StresslessUI
{
    public class exceptionHandler
    {
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            Exception ex = (Exception)args.ExceptionObject;
                logger.Error<Exception>(ex.Message, ex);

            MessageBox.Show(ex.Message + ex);
        }
    }
}
