using System.Windows;
using Serilog;

namespace RPLidar4Net.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(@"E:\UserData\amael\OneDrive\Electronique\RPLIDAR A1\Scan Data\RPLidar4Net\logs.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            Log.Information("OnStartup");
        }
    }
}
