using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingCsvAnalyser.Appilication;
using TradingCsvAnalyser.Extensions;
using TradingCsvAnalyser.Models.Database;
using TradingCsvAnalyser.Windows;

namespace TradingCsvAnalyser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(typeof(Configuration), _ => Configuration);
            serviceCollection.AddDbContext<AnalyserContext>(ServiceLifetime.Transient);
            serviceCollection.ConfigureServices();
            
            ServiceProvider = serviceCollection.BuildServiceProvider();
            
            ServiceProvider.GetRequiredService<OverView>().Show();
            
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled Exception just occured:"+ e.Exception.Message, "Error");
            e.Handled = true;
        }
    }
}