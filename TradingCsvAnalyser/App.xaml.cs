using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            serviceCollection.AddDbContextFactory<AnalyserContext>(new AnalyserConfig(Configuration));
            serviceCollection.ConfigureServices();
            
            ServiceProvider = serviceCollection.BuildServiceProvider();

            ServiceProvider.GetRequiredService<OverView>().Show();
            
        }
    }
}