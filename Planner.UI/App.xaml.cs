using Autofac;
using Planner.UI.Startup;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Planner.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var container = Bootstrapper.Bootstrap();

            CultureInfo c = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = c;

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured." + Environment.NewLine + e.Exception.Message, "Unexpected error");
            e.Handled = true;
        }

    }
}
