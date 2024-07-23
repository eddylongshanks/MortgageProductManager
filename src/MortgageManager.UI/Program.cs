

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MortgageManager.CMS.Mappers;
using MortgageManager.CMS;
using MortgageManager.DataAccess.Helpers;

namespace MortgageManager.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<Form1>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddLogging(builder => builder
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddConsole()
                    );
                    services.AddSingleton<CsvManager>();
                    services.AddSingleton<MortgageCreator>();
                    services.AddSingleton<IProductMortgageMapper, ProductMortgageMapper>();
                    services.AddTransient<Form1>();
                });
        }

    }
}